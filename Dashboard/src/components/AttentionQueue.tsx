import {
  AttentionEntry,
  AttentionReason,
  formatDuration,
} from "../lib/analytics";

interface Props {
  queue: AttentionEntry[];
}

interface ReasonStyle {
  icon: string;
  label: string;
  badge: string;
  row: string;
}

const REASON_STYLES: Record<AttentionReason, ReasonStyle> = {
  urgent: {
    icon: "🚨",
    label: "precisa de ajuda",
    badge: "bg-rose-900 text-rose-200",
    row: "bg-rose-950/40 border-rose-900",
  },
  help: {
    icon: "🆘",
    label: "pediu ajuda",
    badge: "bg-amber-900 text-amber-200",
    row: "bg-amber-950/30 border-amber-900/60",
  },
  "stuck-long": {
    icon: "⏱️",
    label: "preso há muito tempo",
    badge: "bg-rose-900/70 text-rose-200",
    row: "bg-rose-950/30 border-rose-900/60",
  },
  stuck: {
    icon: "⏱️",
    label: "sem mexer",
    badge: "bg-amber-900/60 text-amber-200",
    row: "bg-gray-900 border-gray-800",
  },
  slow: {
    icon: "🐢",
    label: "lento neste passo",
    badge: "bg-violet-900/60 text-violet-200",
    row: "bg-gray-900 border-gray-800",
  },
};

function detail(e: AttentionEntry): string {
  const mins = `${Math.round(e.stuckMinutes)}min`;
  if (e.reason === "slow" && e.avgStepSeconds) {
    return `há ${mins} (média ${formatDuration(e.avgStepSeconds)})`;
  }
  return `há ${mins}`;
}

export function AttentionQueue({ queue }: Props) {
  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg h-full">
      <h2 className="text-lg font-semibold text-gray-100 mb-1">Atenção agora</h2>
      <p className="text-xs text-gray-500 mb-4">
        {queue.length === 0
          ? "Ninguém precisa de ti"
          : `${queue.length} ${queue.length === 1 ? "miúdo" : "miúdos"} a precisar de ti`}
      </p>

      {queue.length === 0 ? (
        <p className="text-sm text-gray-500">Toda a malta está a avançar 🎉</p>
      ) : (
        <ul className="space-y-2">
          {queue.map((e) => {
            const style = REASON_STYLES[e.reason];
            return (
              <li
                key={e.nameId}
                className={`flex items-center justify-between gap-3 p-3 rounded-lg border ${style.row}`}
              >
                <div className="flex items-center gap-3 min-w-0">
                  <span className="text-xl shrink-0" aria-hidden>
                    {style.icon}
                  </span>
                  <div className="min-w-0">
                    <div className="text-gray-100 font-medium truncate">{e.name}</div>
                    <div className="text-xs text-gray-500 font-mono truncate">{e.stepId}</div>
                  </div>
                </div>
                <div className="text-right shrink-0">
                  <span className={`text-xs px-2 py-1 rounded ${style.badge}`}>
                    {style.label}
                  </span>
                  <div className="text-xs text-gray-500 mt-1">{detail(e)}</div>
                </div>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
}
