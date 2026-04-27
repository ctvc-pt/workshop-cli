import { useSheetData } from "./hooks/useSheetData";
import {
  isCompleted,
  totalDurationByKid,
  formatDuration,
  minutesSince,
} from "./lib/analytics";
import { Header } from "./components/Header";
import { StatCard } from "./components/StatCard";
import { StuckTable } from "./components/StuckTable";
import { HelpQueue } from "./components/HelpQueue";
import { StepDistribution } from "./components/StepDistribution";
import { SlowestSteps } from "./components/SlowestSteps";

export default function App() {
  const { sessions, timeline, help, error, lastRefresh } = useSheetData(5000);

  const total = sessions.length;
  const finished = sessions.filter((s) => isCompleted(s.stepId)).length;
  const completionPct = total > 0 ? Math.round((finished / total) * 100) : 0;
  const active = sessions.filter(
    (s) => !isCompleted(s.stepId) && minutesSince(s.timestamp) < 30,
  ).length;

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
          <StatCard label="Em curso" value={active} accent="amber" />
          <StatCard
            label="Duração média"
            value={formatDuration(avgDuration)}
            hint={durations.length > 0 ? `${durations.length} miúdos` : "sem dados"}
            accent="violet"
          />
        </div>

        <div className="mb-6">
          <StepDistribution sessions={sessions} />
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
          <StuckTable sessions={sessions} />
          <HelpQueue help={help} />
        </div>

        <SlowestSteps timeline={timeline} />
      </div>
    </div>
  );
}
