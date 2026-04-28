import { Session } from "../lib/sheets";
import { distributionByStep, medianStep, namesByStep, countBehind } from "../lib/analytics";
import { Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis, Cell } from "recharts";

interface Props {
  sessions: Session[];
}

interface TooltipPayload {
  active?: boolean;
  payload?: Array<{ payload: { stepId: string; count: number; names: string[] } }>;
}

function CustomTooltip({ active, payload }: TooltipPayload) {
  if (!active || !payload?.length) return null;
  const { stepId, count, names } = payload[0].payload;
  return (
    <div className="bg-gray-900 border border-gray-700 rounded-lg p-3 shadow-xl max-w-xs">
      <div className="text-xs font-mono text-gray-400 mb-1">{stepId}</div>
      <div className="text-sm text-gray-100 font-semibold mb-2">
        {count} {count === 1 ? "miúdo" : "miúdos"}
      </div>
      <ul className="text-xs text-gray-300 space-y-0.5">
        {names.map((n, i) => (
          <li key={i}>{n}</li>
        ))}
      </ul>
    </div>
  );
}

export function StepDistribution({ sessions }: Props) {
  const dist = distributionByStep(sessions);
  const names = namesByStep(sessions);
  const data = dist.map((d) => ({ ...d, names: names.get(d.stepId) ?? [] }));

  const median = medianStep(sessions);
  const behind = median ? countBehind(sessions, median) : 0;

  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg">
      <div className="flex items-baseline justify-between mb-1">
        <h2 className="text-lg font-semibold text-gray-100">Distribuição por step</h2>
        {median && (
          <span className="text-xs text-gray-400">
            Mediana da turma:{" "}
            <span className="font-mono text-emerald-400">{median}</span>
            {behind > 0 && (
              <span className="text-gray-500"> · {behind} atrás</span>
            )}
          </span>
        )}
      </div>
      <p className="text-xs text-gray-500 mb-4">
        Quantos miúdos em cada parte do guide (passa o rato para ver nomes)
      </p>

      {data.length === 0 ? (
        <p className="text-sm text-gray-500">Sem dados ainda.</p>
      ) : (
        <ResponsiveContainer width="100%" height={260}>
          <BarChart data={data} margin={{ top: 8, right: 8, bottom: 8, left: 8 }}>
            <XAxis
              dataKey="stepId"
              stroke="#6b7280"
              tick={{ fill: "#9ca3af", fontSize: 11 }}
              angle={-35}
              textAnchor="end"
              height={70}
              interval={0}
            />
            <YAxis stroke="#6b7280" tick={{ fill: "#9ca3af", fontSize: 11 }} allowDecimals={false} />
            <Tooltip content={<CustomTooltip />} cursor={{ fill: "#1f2937" }} />
            <Bar dataKey="count" radius={[4, 4, 0, 0]}>
              {data.map((d, i) => (
                <Cell key={i} fill={d.stepId === median ? "#34d399" : "#38bdf8"} />
              ))}
            </Bar>
          </BarChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}
