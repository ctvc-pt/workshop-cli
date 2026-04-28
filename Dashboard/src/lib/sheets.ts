// Lê as sheets do Google directamente em CSV via gviz. Funciona desde que a
// spreadsheet esteja partilhada como "Anyone with the link can view".

const SPREADSHEET_ID = "1njSnYweZnBLdnoUnJjJFSO73pb7huFEOxt2J3Bc4BYw";

export interface Session {
  name: string;
  age: string;
  email: string;
  stepId: string;
  nameId: string;
  timestamp: string;
}

export interface TimelineEntry {
  nameId: string;
  stepId: string;
  timestamp: string;
}

export interface HelpEntry {
  name: string;
  stepId: string;
  status: string;
  timestamp: string;
}

const sheetUrl = (sheet: string) =>
  `https://docs.google.com/spreadsheets/d/${SPREADSHEET_ID}/gviz/tq?tqx=out:csv&sheet=${encodeURIComponent(sheet)}`;

async function fetchSheet(sheet: string): Promise<string[][]> {
  // Cache-bust com timestamp — Google cacha o CSV ~1min senao.
  const res = await fetch(`${sheetUrl(sheet)}&_=${Date.now()}`);
  if (!res.ok) throw new Error(`${sheet}: HTTP ${res.status}`);
  return parseCsv(await res.text());
}

function parseCsv(text: string): string[][] {
  const rows: string[][] = [];
  let row: string[] = [];
  let field = "";
  let inQuotes = false;
  for (let i = 0; i < text.length; i++) {
    const c = text[i];
    if (inQuotes) {
      if (c === '"') {
        if (text[i + 1] === '"') { field += '"'; i++; }
        else { inQuotes = false; }
      } else { field += c; }
    } else {
      if (c === '"') inQuotes = true;
      else if (c === ",") { row.push(field); field = ""; }
      else if (c === "\n") { row.push(field); rows.push(row); row = []; field = ""; }
      else if (c === "\r") { /* skip */ }
      else field += c;
    }
  }
  if (field.length > 0 || row.length > 0) { row.push(field); rows.push(row); }
  return rows;
}

export async function fetchSessions(): Promise<Session[]> {
  const rows = await fetchSheet("Sessions");
  return rows.slice(1).filter((r) => r.length >= 5 && r[4]).map((r) => ({
    name: r[0] ?? "",
    age: r[1] ?? "",
    email: r[2] ?? "",
    stepId: r[3] ?? "",
    nameId: r[4] ?? "",
    timestamp: r[5] ?? "",
  }));
}

export async function fetchTimeline(): Promise<TimelineEntry[]> {
  try {
    const rows = await fetchSheet("Timeline");
    return rows.slice(1).filter((r) => r.length >= 3 && r[0]).map((r) => ({
      nameId: r[0] ?? "",
      stepId: r[1] ?? "",
      timestamp: r[2] ?? "",
    }));
  } catch {
    // Se a sheet Timeline ainda nao foi criada, voltamos vazio em vez de partir.
    return [];
  }
}

export async function fetchHelp(): Promise<HelpEntry[]> {
  try {
    const rows = await fetchSheet("Ajudas");
    return rows.slice(1).filter((r) => r.length >= 1 && r[0]).map((r) => ({
      name: r[0] ?? "",
      stepId: r[1] ?? "",
      status: r[2] ?? "",
      timestamp: r[3] ?? "",
    }));
  } catch {
    return [];
  }
}
