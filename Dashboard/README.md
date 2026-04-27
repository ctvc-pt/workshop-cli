# Workshop Dashboard

Dashboard local para o monitor acompanhar um workshop em tempo real. Lê os dados
directamente do Google Sheets (sem backend), refresca a cada 5s.

## Pré-requisitos

1. **Spreadsheet partilhada como "Anyone with the link can view"**. O dashboard
   usa o endpoint público `gviz` do Google Sheets que requer este nível de
   partilha. Sem isto, o fetch devolve HTML em vez de CSV.

2. **Sheet `Timeline` criada manualmente** (ver `Documentation/HowToSendEmailViaN8n.md`
   ou `CsvController.cs`). Headers em A1:C1: `nameId | stepId | timestamp`.

3. **Coluna F na sheet `Sessions`** com header `Timestamp` (a 6ª coluna). O CLI
   passou a escrever timestamp ISO aqui em cada step.

4. **Node 18+** instalado.

## Correr localmente

```bash
cd Dashboard
npm install
npm run dev
```

Abre `http://localhost:5173` no browser. O dashboard refresca sozinho.

## Build de produção

```bash
npm run build
```

Os ficheiros vão para `dist/`. Podes servir com `npm run preview` ou copiar para
qualquer host estático (Vercel, Netlify, GitHub Pages).

## Configurar para outro spreadsheet

Edita `src/lib/sheets.ts:5` — variável `SPREADSHEET_ID`.

## O que cada widget faz

| Widget | Lê de | Mostra |
|---|---|---|
| Total inscritos | Sessions | nº de miúdos com sessão |
| Concluíram | Sessions | quantos chegaram ao último step (e %) |
| Em curso | Sessions | activos (sem ser concluídos, mexeram nos últimos 30min) |
| Duração média | Timeline | tempo médio do primeiro ao último step por miúdo |
| Distribuição por step | Sessions | bar chart de quantos estão em cada step |
| Quem está preso | Sessions | miúdos sem mexer há >5min, ordenados |
| Ajudas pendentes | Ajudas | pedidos de ajuda não resolvidos |
| Steps mais demorados | Timeline | top 5 steps por tempo médio (analytics post-workshop) |

## Identificar passos finais

A heurística vive em `src/lib/analytics.ts:5` (`FINAL_STEP_REGEXES`). Por defeito
considera "concluído" qualquer stepId que comece por `030-`/`029-` ou termine
em `-end`/`-fim`. Atualiza se adicionares guides com IDs diferentes.
