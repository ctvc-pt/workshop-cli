import { TimelineEntry } from "../lib/sheets";
import { avgDurationByStep, formatDuration } from "../lib/analytics";

interface Props {
  timeline: TimelineEntry[];
  topN?: number;
}

export function SlowestSteps({ timeline, topN = 5 }: Props) {
  const data = avgDurationByStep(timeline).slice(0, topN);
  const max = data[0]?.avgSeconds ?? 1;

  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg">
      <h2 className="text-lg font-semibold text-gray-100 mb-1">Steps mais demorados</h2>
      <p className="text-xs text-gray-500 mb-4">Tempo médio por step (top {topN})</p>

      {data.length === 0 ? (
        <p className="text-sm text-gray-500">Ainda não há histórico suficiente.</p>
      ) : (
        <ul className="space-y-3">
          {data.map((d, i) => (
            <li key={d.stepId}>
              <div className="flex items-baseline justify-between mb-1">
                <span className="text-sm text-gray-300">
                  <span className="text-gray-500 mr-2">#{i + 1}</span>
                  <span className="font-mono text-xs">{d.stepId}</span>
                </span>
                <span className="text-sm text-violet-400 font-semibold">
                  {formatDuration(d.avgSeconds)}
                </span>
              </div>
              <div className="h-2 bg-gray-800 rounded-full overflow-hidden">
                <div
                  className="h-full bg-violet-500 rounded-full"
                  style={{ width: `${(d.avgSeconds / max) * 100}%` }}
                />
              </div>
              <div className="text-xs text-gray-500 mt-1">
                {d.samples} {d.samples === 1 ? "amostra" : "amostras"}
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
