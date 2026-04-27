interface StatCardProps {
  label: string;
  value: string | number;
  hint?: string;
  accent?: "emerald" | "rose" | "amber" | "sky" | "violet";
}

const accentColors: Record<NonNullable<StatCardProps["accent"]>, string> = {
  emerald: "text-emerald-400",
  rose: "text-rose-400",
  amber: "text-amber-400",
  sky: "text-sky-400",
  violet: "text-violet-400",
};

export function StatCard({ label, value, hint, accent = "sky" }: StatCardProps) {
  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-6 shadow-lg">
      <div className="text-xs uppercase tracking-wider text-gray-500 mb-2">{label}</div>
      <div className={`text-4xl font-bold ${accentColors[accent]}`}>{value}</div>
      {hint && <div className="text-xs text-gray-500 mt-2">{hint}</div>}
    </div>
  );
}
