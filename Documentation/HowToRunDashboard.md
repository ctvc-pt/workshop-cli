# How to Run the Dashboard

Real-time monitoring dashboard for the workshop monitor. Shows where each kid
is in the guide, who's stuck, who needs help, and post-workshop analytics
(completion rate, slowest steps).

## What it shows

The dashboard is a single page split into three zones:

### Top: KPIs at a glance
- **Total inscritos** — number of kids with an active session
- **Concluíram** — kids who reached the final step (and percentage)
- **Em curso** — kids actively working (mexeram nos últimos 30min)
- **Duração média** — mean total time from first to last step, per kid

### Middle: live operational widgets
- **Distribuição por step** — bar chart, how many kids are sitting on each
  step. Tells you at a glance whether the class is bunched up or spread out.
- **Quem está preso** — kids whose last activity was more than 5 minutes ago,
  sorted by stuckness. Anyone in red (>10 min) probably needs you.
- **Ajudas pendentes** — kids who typed `ajuda` and haven't been resolved.
  "URGENTE" badge means the monitor (you) marked them as needing teacher help.

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
| `Ajudas` | `CsvController.GetHelp` + `PrintHelp` (CLI) | help requests + monitor responses |

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

- **Quem está preso** — scan this every couple of minutes. Anyone red (>10
  min) is probably stuck on something they don't know how to ask about. Walk
  over.
- **Ajudas pendentes** — if a kid pops up here, they explicitly asked for
  help. Resolve from the spreadsheet (mark the row green) or talk to them.
- **Distribuição por step** — if the class is supposed to be at step
  `015-gravidade` but the bar at `010-code` is huge, you're losing them
  somewhere between those steps.

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
| "Stuck" threshold | `Dashboard/src/App.tsx` (passed to `<StuckTable thresholdMin={5} />`) |
| Final-step patterns | `Dashboard/src/lib/analytics.ts:5` (`FINAL_STEP_REGEXES`) |

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
