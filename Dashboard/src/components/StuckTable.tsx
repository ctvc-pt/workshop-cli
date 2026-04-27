import { Session } from "../lib/sheets";
import { minutesSince } from "../lib/analytics";

interface Props {
  sessions: Session[];
  thresholdMin?: number;
}

export function StuckTable({ sessions, thresholdMin = 5 }: Props) {
  const stuck = sessions
    .map((s) => ({ ...s, mins: minutesSince(s.timestamp) }))
    .filter((s) => s.mins >= thresholdMin && Number.isFinite(s.mins))
    .sort((a, b) => b.mins - a.mins);

  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg h-full">
      <h2 className="text-lg font-semibold text-gray-100 mb-1">Quem está preso</h2>
      <p className="text-xs text-gray-500 mb-4">Sem mexer há mais de {thresholdMin} minutos</p>

      {stuck.length === 0 ? (
        <p className="text-sm text-gray-500">Toda a malta está a avançar 🎉</p>
      ) : (
        <table className="w-full text-sm">
          <thead>
            <tr className="text-left text-gray-500 border-b border-gray-800">
              <th className="py-2 font-normal">Miúdo</th>
              <th className="py-2 font-normal">Step</th>
              <th className="py-2 font-normal text-right">Há</th>
            </tr>
          </thead>
          <tbody>
            {stuck.map((s) => (
              <tr key={s.nameId} className="border-b border-gray-800/50 last:border-0">
                <td className="py-2 text-gray-200">{s.name || s.nameId}</td>
                <td className="py-2 text-gray-400 font-mono text-xs">{s.stepId}</td>
                <td className="py-2 text-right">
                  <span className={s.mins > 10 ? "text-rose-400 font-semibold" : "text-amber-400"}>
                    {Math.round(s.mins)}min
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
