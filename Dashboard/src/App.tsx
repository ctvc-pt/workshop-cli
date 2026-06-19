import { useSheetData } from "./hooks/useSheetData";
import {
  isCompleted,
  totalDurationByKid,
  formatDuration,
  buildAttentionQueue,
} from "./lib/analytics";
import { Header } from "./components/Header";
import { StatCard } from "./components/StatCard";
import { AttentionQueue } from "./components/AttentionQueue";
import { StepDistribution } from "./components/StepDistribution";
import { SlowestSteps } from "./components/SlowestSteps";

const TOP_NAMES_IN_KPI = 3;

export default function App() {
  const { sessions, timeline, help, error, lastRefresh } = useSheetData(5000);

  const total = sessions.length;
  const finished = sessions.filter((s) => isCompleted(s.stepId)).length;
  const completionPct = total > 0 ? Math.round((finished / total) * 100) : 0;

  const attentionQueue = buildAttentionQueue(sessions, help, timeline);
  const pending = attentionQueue.length;
  const topNames = attentionQueue
    .slice(0, TOP_NAMES_IN_KPI)
    .map((e) => e.name)
    .join(", ");
  const pendingHint =
    pending === 0
      ? "tudo a correr bem"
      : pending <= TOP_NAMES_IN_KPI
        ? topNames
        : `${topNames} +${pending - TOP_NAMES_IN_KPI}`;

  const durations = totalDurationByKid(timeline);
  const avgDuration =
    durations.length > 0 ? durations.reduce((a, b) => a + b, 0) / durations.length : 0;

  return (
    <div className="min-h-full bg-gray-950 text-gray-100 p-6 md:p-8">
      <div className="max-w-7xl mx-auto">
        <Header lastRefresh={lastRefresh} error={error} />

        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
          <StatCard label="Total inscritos" value={total} accent="sky" />
          <StatCard
            label="Concluíram"
            value={`${finished} (${completionPct}%)`}
            accent="emerald"
          />
          <StatCard
            label="À tua espera"
            value={pending}
            hint={pendingHint}
            accent={pending > 0 ? "rose" : "emerald"}
          />
          <StatCard
            label="Duração média"
            value={formatDuration(avgDuration)}
            hint={durations.length > 0 ? `${durations.length} miúdos` : "sem dados"}
            accent="violet"
          />
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
          <AttentionQueue queue={attentionQueue} />
          <StepDistribution sessions={sessions} />
        </div>

        <SlowestSteps timeline={timeline} />
      </div>
    </div>
  );
}
