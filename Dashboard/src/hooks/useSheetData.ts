import { useEffect, useState } from "react";
import {
  fetchSessions,
  fetchTimeline,
  fetchHelp,
  Session,
  TimelineEntry,
  HelpEntry,
} from "../lib/sheets";

export interface SheetData {
  sessions: Session[];
  timeline: TimelineEntry[];
  help: HelpEntry[];
  loading: boolean;
  error: string | null;
  lastRefresh: Date | null;
}

export function useSheetData(intervalMs = 5000): SheetData {
  const [sessions, setSessions] = useState<Session[]>([]);
  const [timeline, setTimeline] = useState<TimelineEntry[]>([]);
  const [help, setHelp] = useState<HelpEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [lastRefresh, setLastRefresh] = useState<Date | null>(null);

  useEffect(() => {
    let cancelled = false;
    async function load() {
      try {
        const [s, t, h] = await Promise.all([
          fetchSessions(),
          fetchTimeline(),
          fetchHelp(),
        ]);
        if (cancelled) return;
        setSessions(s);
        setTimeline(t);
        setHelp(h);
        setError(null);
        setLastRefresh(new Date());
      } catch (e) {
        if (cancelled) return;
        setError(e instanceof Error ? e.message : "falha a carregar dados");
      } finally {
        if (!cancelled) setLoading(false);
      }
    }
    load();
    const id = setInterval(load, intervalMs);
    return () => {
      cancelled = true;
      clearInterval(id);
    };
  }, [intervalMs]);

  return { sessions, timeline, help, loading, error, lastRefresh };
}
