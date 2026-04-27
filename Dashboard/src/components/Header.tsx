interface Props {
  lastRefresh: Date | null;
  error: string | null;
}

export function Header({ lastRefresh, error }: Props) {
  return (
    <header className="flex items-center justify-between mb-8">
      <div>
        <h1 className="text-2xl font-bold text-gray-100">Workshop CTVC</h1>
        <p className="text-sm text-gray-500">Dashboard de monitorização</p>
      </div>
      <div className="text-right">
        {error ? (
          <span className="text-sm text-rose-400">⚠ {error}</span>
        ) : lastRefresh ? (
          <span className="text-xs text-gray-500">
            Atualizado às {lastRefresh.toLocaleTimeString("pt-PT")}
          </span>
        ) : (
          <span className="text-xs text-gray-500">A carregar…</span>
        )}
      </div>
    </header>
  );
}
