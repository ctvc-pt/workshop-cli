import { Session, TimelineEntry } from "./sheets";

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

export function formatDuration(seconds: number): string {
  if (!Number.isFinite(seconds) || seconds < 0) return "—";
  if (seconds < 60) return `${Math.round(seconds)}s`;
  const m = Math.floor(seconds / 60);
  const s = Math.round(seconds % 60);
  if (m < 60) return s === 0 ? `${m}min` : `${m}min ${s}s`;
  const h = Math.floor(m / 60);
  return `${h}h ${m % 60}min`;
}
