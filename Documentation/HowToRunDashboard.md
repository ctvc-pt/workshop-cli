# How to Run the Dashboard

Real-time monitoring dashboard for the workshop monitor. Shows where each kid
is in the guide, who's stuck, who needs help, and post-workshop analytics
(completion rate, slowest steps).

## What it shows

The dashboard is a single page split into three zones:

### Top: KPIs at a glance
- **Total inscritos** — number of kids with an active session
- **Concluíram** — kids who reached the final step (and percentage)
- **À tua espera** — kids who need attention right now (open help request,
  stuck >5 min, or unusually slow on the current step). The single number
  you scan when you walk into the room.
- **Duração média** — mean total time from first to last step, per kid

### Middle: live operational widgets
- **Atenção agora** — unified queue of every kid that needs you, ordered by
  urgency: 🚨 marked "precisa de ajuda" → 🆘 typed `ajuda` (pending) → ⏱️ stuck
  >10 min → ⏱️ stuck >5 min → 🐢 unusually slow on this step (>2× the running
  average for it). One row per kid, with the step they're on and how long
  they've been there.
- **Distribuição por step** — bar chart, how many kids are sitting on each
  step. The green bar is the median step (mid-point of the class). Hover any
  bar to see the names of kids on that step.

### Bottom: post-workshop analytics
- **Steps mais demorados** — average time per step across all kids who passed
  through it. Useful after the workshop to identify which parts of the guide
  are slowing everyone down.

## How it gets the data

The dashboard reads three Google Sheets directly from the browser, no backend:

| Sheet | Written by | Used for |
|---|---|---|
| `Sessions` | `CsvController.UpdateSession` (CLI) | current state per kid |
| `Timeline` | `CsvController.AppendTimeline` (CLI) | step-by-step history for analytics |
| `Ajudas` | `CsvController.PrintHelp` (CLI) | help requests + monitor responses (append-only, one row per request: A=name, B=step, C=status, D=timestamp) |

It uses Google's `gviz` CSV endpoint, refreshing every 5 seconds.

## Prerequisites

These are one-time setup steps. If the dashboard ever stops loading, check
these first.

1. **Spreadsheet shared as "Anyone with the link can view"**  
   Open the spreadsheet → Share → General access → "Anyone with the link" →
   Viewer. Without this the gviz endpoint redirects to login and CORS blocks
   the fetch.

2. **`Timeline` sheet must exist**  
   In the spreadsheet, create a tab named exactly `Timeline` with these
   headers in row 1: `nameId | stepId | timestamp`. The CLI appends one row
   here per step transition.

3. **`Sessions` sheet must have a 6th column**  
   Add a header `Timestamp` in column F. The CLI writes ISO timestamps there.

4. **Node.js 18 or higher** on the monitor's laptop. Download from
   <https://nodejs.org> if missing.

## How to start it

The simplest way is to **let `start_receiver.bat` do it for you**. The bat now
launches the dashboard in a separate window and opens the browser
automatically:

```
ToolsAfterWorkshop\Receiver\start_receiver.bat
```

Sequence on first run:
1. Bat checks Python and Node are installed
2. Installs the dashboard's npm dependencies (only once, on first run, ~30s)
3. Launches Vite dev server in a new console window
4. Waits 4 seconds, then opens `http://localhost:5173` in your default browser
5. Continues with the receiver (Flask) in the main window

If Node isn't installed, the bat skips the dashboard and arranca only the
receiver. The receiver is the critical piece — kids can still finish the
workshop without the dashboard, just without monitoring.

### Manual start (if you don't want the receiver)

If you only want the dashboard (e.g., to review analytics after a workshop):

```
cd Dashboard
npm install     # first time only
npm run dev
```

Then open <http://localhost:5173>.

## During the workshop — what to watch

- **Atenção agora** — scan this every couple of minutes. Top of the list is
  always your most urgent: 🚨 then 🆘 then ⏱️ then 🐢. If the queue is empty,
  the class is fine.
- **Distribuição por step** — green bar is the class median. If the bars
  before it are tall, the class is stretched out and a few kids are dragging.
  Hover any bar to see exactly who's on that step.

## After the workshop — what to look at

- **Concluíram** — completion rate as a single number.
- **Steps mais demorados** — top 5 bottlenecks in the guide. If `015-gravidade`
  averaged 12 minutes and the next slowest is at 4 minutes, that step needs
  better explanation, more examples, or to be split.
- **Duração média** — typical workshop length. Useful for planning the next
  one.

## Configuration

| What | Where |
|---|---|
| Spreadsheet ID | `Dashboard/src/lib/sheets.ts:5` |
| Refresh interval | `Dashboard/src/App.tsx` (call to `useSheetData(5000)`) |
| "Stuck" threshold | `Dashboard/src/App.tsx` (passed to `<AttentionQueue thresholdMin={5} />`) |
| "Slow" floor | `Dashboard/src/lib/analytics.ts` (`SLOW_MIN_AVG_SECONDS`) |
| Final-step patterns | `Dashboard/src/lib/analytics.ts` (`FINAL_STEP_REGEXES`) |

## Troubleshooting

### Console shows "Access to fetch ... blocked by CORS policy"
The spreadsheet isn't public. See prerequisite 1 above. Pro tip: open the
spreadsheet URL in an incognito window — if it asks you to log in, the share
setting is wrong.

### Numbers all show 0 / charts empty
The CLI hasn't written any rows yet. Run a test session through the CLI and
refresh.

### "Steps mais demorados" stays empty
The `Timeline` sheet doesn't exist or is misspelled. The dashboard fails
silently for this sheet (so the rest of the dashboard keeps working). See
prerequisite 2.

### Dashboard window opens but Vite doesn't start
Most likely Node version is too old. Run `node -v` — needs to be 18 or higher.

### Dashboard launched but browser didn't open
Open <http://localhost:5173> manually. The 4-second delay in the bat is a
guess; on slower machines Vite may take longer to start.

### Want to share the dashboard with another monitor on the network
Vite already listens on `0.0.0.0` (see `Dashboard/vite.config.ts`). On the
other laptop, open `http://<monitor-ip>:5173` (use one of the IPs printed
when `start_receiver.bat` arranca).
