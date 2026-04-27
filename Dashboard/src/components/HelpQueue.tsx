import { HelpEntry } from "../lib/sheets";

interface Props {
  help: HelpEntry[];
}

export function HelpQueue({ help }: Props) {
  // Pendentes = sem status "Resolvido". Inclui os que pediram ajuda mas ainda
  // nao foram atendidos (status vazio) e os explicitos "Precisa de ajuda".
  const pending = help.filter((h) => h.status.toLowerCase() !== "resolvido");

  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg h-full">
      <h2 className="text-lg font-semibold text-gray-100 mb-1">Ajudas pendentes</h2>
      <p className="text-xs text-gray-500 mb-4">{pending.length} por atender</p>

      {pending.length === 0 ? (
        <p className="text-sm text-gray-500">Sem pedidos de ajuda 👌</p>
      ) : (
        <ul className="space-y-2">
          {pending.map((h, i) => {
            const urgent = h.status.toLowerCase().includes("precisa");
            return (
              <li
                key={`${h.name}-${i}`}
                className={`flex items-center justify-between p-3 rounded-lg border ${
                  urgent
                    ? "bg-rose-950/30 border-rose-900"
                    : "bg-amber-950/20 border-amber-900/50"
                }`}
              >
                <div>
                  <div className="text-gray-100 font-medium">{h.name}</div>
                  <div className="text-xs text-gray-500 font-mono">{h.stepId}</div>
                </div>
                <span
                  className={`text-xs px-2 py-1 rounded ${
                    urgent ? "bg-rose-900 text-rose-200" : "bg-amber-900/60 text-amber-200"
                  }`}
                >
                  {urgent ? "URGENTE" : "pendente"}
                </span>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
}
