import { Session } from "../lib/sheets";
import { distributionByStep } from "../lib/analytics";
import { Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis, Cell } from "recharts";

interface Props {
  sessions: Session[];
}

export function StepDistribution({ sessions }: Props) {
  const data = distributionByStep(sessions);

  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg">
      <h2 className="text-lg font-semibold text-gray-100 mb-1">Distribuição por step</h2>
      <p className="text-xs text-gray-500 mb-4">Quantos miúdos em cada parte do guide</p>

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
            <Tooltip
              contentStyle={{ background: "#111827", border: "1px solid #1f2937", borderRadius: 8 }}
              labelStyle={{ color: "#e5e7eb" }}
            />
            <Bar dataKey="count" radius={[4, 4, 0, 0]}>
              {data.map((_, i) => (
                <Cell key={i} fill="#38bdf8" />
              ))}
            </Bar>
          </BarChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}
