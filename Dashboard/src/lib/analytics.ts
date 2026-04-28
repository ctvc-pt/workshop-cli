import { HelpEntry, Session, TimelineEntry } from "./sheets";

// Padroes que identificam o ultimo step de qualquer guide. Atualiza se
// adicionares guides com IDs finais diferentes.
const FINAL_STEP_REGEXES = [/^030-/, /^029-/, /-end$/i, /-fim$/i];

export function isCompleted(stepId: string): boolean {
  return FINAL_STEP_REGEXES.some((r) => r.test(stepId));
}

export function minutesSince(iso: string): number {
  if (!iso) return Number.POSITIVE_INFINITY;
  const t = Date.parse(iso);
  if (Number.isNaN(t)) return Number.POSITIVE_INFINITY;
  return (Date.now() - t) / 60000;
}

export interface StepStat {
  stepId: string;
  count: number;
}

export function distributionByStep(sessions: Session[]): StepStat[] {
  const counts = new Map<string, number>();
  for (const s of sessions) {
    if (!s.stepId) continue;
    counts.set(s.stepId, (counts.get(s.stepId) ?? 0) + 1);
  }
  return Array.from(counts.entries())
    .map(([stepId, count]) => ({ stepId, count }))
    .sort((a, b) => a.stepId.localeCompare(b.stepId));
}

export interface StepDuration {
  stepId: string;
  avgSeconds: number;
  samples: number;
}

// Para cada miudo, ordena as suas entries do Timeline por timestamp e calcula
// quanto tempo passou entre step N e step N+1 (esse e o "tempo no step N").
export function avgDurationByStep(timeline: TimelineEntry[]): StepDuration[] {
  const byKid = new Map<string, TimelineEntry[]>();
  for (const e of timeline) {
    if (!e.nameId || !e.stepId || !e.timestamp) continue;
    const list = byKid.get(e.nameId) ?? [];
    list.push(e);
    byKid.set(e.nameId, list);
  }

  const sums = new Map<string, { total: number; n: number }>();
  for (const list of byKid.values()) {
    list.sort((a, b) => a.timestamp.localeCompare(b.timestamp));
    for (let i = 0; i < list.length - 1; i++) {
      const start = Date.parse(list[i].timestamp);
      const end = Date.parse(list[i + 1].timestamp);
      if (Number.isNaN(start) || Number.isNaN(end) || end <= start) continue;
      const seconds = (end - start) / 1000;
      const cur = sums.get(list[i].stepId) ?? { total: 0, n: 0 };
      cur.total += seconds;
      cur.n += 1;
      sums.set(list[i].stepId, cur);
    }
  }

  return Array.from(sums.entries())
    .map(([stepId, { total, n }]) => ({
      stepId,
      avgSeconds: total / n,
      samples: n,
    }))
    .sort((a, b) => b.avgSeconds - a.avgSeconds);
}

export function totalDurationByKid(timeline: TimelineEntry[]): number[] {
  const byKid = new Map<string, TimelineEntry[]>();
  for (const e of timeline) {
    if (!e.nameId || !e.timestamp) continue;
    const list = byKid.get(e.nameId) ?? [];
    list.push(e);
    byKid.set(e.nameId, list);
  }
  const durations: number[] = [];
  for (const list of byKid.values()) {
    if (list.length < 2) continue;
    list.sort((a, b) => a.timestamp.localeCompare(b.timestamp));
    const start = Date.parse(list[0].timestamp);
    const end = Date.parse(list[list.length - 1].timestamp);
    if (Number.isNaN(start) || Number.isNaN(end)) continue;
    durations.push((end - start) / 1000);
  }
  return durations;
}

export type AttentionReason = "urgent" | "help" | "stuck-long" | "stuck" | "slow";

export interface AttentionEntry {
  nameId: string;
  name: string;
  stepId: string;
  stuckMinutes: number;
  hasOpenHelp: boolean;
  isUrgent: boolean;
  isSlow: boolean;
  avgStepSeconds: number | null;
  reason: AttentionReason;
}

// So flagamos como "lento" steps cuja media seja > 30s. Steps muito curtos
// (mensagens informativas) sao demasiado ruidosos para esta heuristica.
const SLOW_MIN_AVG_SECONDS = 30;

// Cruza sessions + ajudas + timeline numa fila ordenada por urgencia. Cada
// miudo aparece no maximo uma vez. So entram miudos com pelo menos um sinal:
// pedido de ajuda em aberto, parado >=5min, ou >2x a media do step.
export function buildAttentionQueue(
  sessions: Session[],
  help: HelpEntry[],
  timeline: TimelineEntry[],
  stuckThresholdMin = 5,
): AttentionEntry[] {
  const lastHelpByKid = new Map<string, HelpEntry>();
  for (const h of help) {
    if (!h.name) continue;
    const prev = lastHelpByKid.get(h.name);
    if (!prev || (h.timestamp && h.timestamp.localeCompare(prev.timestamp) > 0)) {
      lastHelpByKid.set(h.name, h);
    }
  }

  const avgByStep = new Map<string, number>();
  for (const d of avgDurationByStep(timeline)) {
    avgByStep.set(d.stepId, d.avgSeconds);
  }

  const entries: AttentionEntry[] = [];
  for (const s of sessions) {
    if (isCompleted(s.stepId)) continue;

    const stuck = minutesSince(s.timestamp);
    const stuckMin = Number.isFinite(stuck) ? stuck : 0;

    const lastHelp = lastHelpByKid.get(s.nameId);
    const helpStatus = lastHelp?.status.toLowerCase() ?? "";
    const hasOpenHelp = !!lastHelp && helpStatus !== "resolvido";
    const isUrgent = hasOpenHelp && helpStatus.includes("precisa");

    const avgSec = avgByStep.get(s.stepId) ?? null;
    const isSlow =
      avgSec !== null &&
      avgSec >= SLOW_MIN_AVG_SECONDS &&
      stuckMin * 60 > 2 * avgSec;

    if (!hasOpenHelp && stuckMin < stuckThresholdMin && !isSlow) continue;

    let reason: AttentionReason;
    if (isUrgent) reason = "urgent";
    else if (hasOpenHelp) reason = "help";
    else if (stuckMin >= 10) reason = "stuck-long";
    else if (stuckMin >= stuckThresholdMin) reason = "stuck";
    else reason = "slow";

    entries.push({
      nameId: s.nameId,
      name: s.name || s.nameId,
      stepId: s.stepId,
      stuckMinutes: stuckMin,
      hasOpenHelp,
      isUrgent,
      isSlow,
      avgStepSeconds: avgSec,
      reason,
    });
  }

  const bucket = (e: AttentionEntry): number => {
    if (e.reason === "urgent") return 0;
    if (e.reason === "help") return 1;
    if (e.reason === "stuck-long") return 2;
    if (e.reason === "stuck") return 3;
    return 4;
  };

  return entries.sort((a, b) => {
    const ba = bucket(a);
    const bb = bucket(b);
    if (ba !== bb) return ba - bb;
    return b.stuckMinutes - a.stuckMinutes;
  });
}

// Quantos miudos precisam da tua atencao agora (mesmo criterio que a fila).
export function pendingAttentionCount(
  sessions: Session[],
  help: HelpEntry[],
  timeline: TimelineEntry[],
): number {
  return buildAttentionQueue(sessions, help, timeline).length;
}

// Mediana do step actual da turma (so miudos nao concluidos). Util para
// dar uma nocao de pace ao monitor sem precisar de configurar nada.
export function medianStep(sessions: Session[]): string | null {
  const active = sessions.filter((s) => s.stepId && !isCompleted(s.stepId));
  if (active.length === 0) return null;
  const sorted = [...active].sort((a, b) => a.stepId.localeCompare(b.stepId));
  return sorted[Math.floor(sorted.length / 2)].stepId;
}

// Quantos miudos estao ATRAS do step S (stepId < S em ordem alfabetica).
export function countBehind(sessions: Session[], stepId: string): number {
  return sessions.filter(
    (s) => s.stepId && !isCompleted(s.stepId) && s.stepId.localeCompare(stepId) < 0,
  ).length;
}

// Para o tooltip do StepDistribution: lista de nomes em cada step.
export function namesByStep(sessions: Session[]): Map<string, string[]> {
  const map = new Map<string, string[]>();
  for (const s of sessions) {
    if (!s.stepId) continue;
    const list = map.get(s.stepId) ?? [];
    list.push(s.name || s.nameId);
    map.set(s.stepId, list);
  }
  return map;
}

export function formatDuration(seconds: number): string {
  if (!Number.isFinite(seconds) || seconds < 0) return "—";
  if (seconds < 60) return `${Math.round(seconds)}s`;
  const m = Math.floor(seconds / 60);
  const s = Math.round(seconds % 60);
  if (m < 60) return s === 0 ? `${m}min` : `${m}min ${s}s`;
  const h = Math.floor(m / 60);
  return `${h}h ${m % 60}min`;
}
