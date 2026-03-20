# SampleOrg.Foo Load Tests

k6 load tests for the SampleOrg.Foo website. k6 transpiles `.ts` files natively — no build step required.

## Prerequisites

- [k6](https://grafana.com/docs/k6/latest/set-up/install-k6/) installed and on `PATH`
- [Node.js + npm](https://nodejs.org/) for editor TypeScript support (optional but recommended)

## Setup

Install npm packages to get `@types/k6` for TypeScript intellisense in your editor:

```powershell
npm install
```

Create a `config.local.json` file to supply the password (this file is gitignored):

```json
{
  "password": "secret"
}
```

You can also override `baseDomain` or `username` here if needed.

## Running tests

Simple version:

```powershell
k6 run BaseLoadTest.ts
```

With output:

```powershell
k6 run --out "jsonl=test-results/$(Get-Date -Format 'yyyy-MM-ddTHH-mm-ss').jsonl" BaseLoadTest.ts
```

Output files land in `test-results/` (gitignored) as newline-delimited JSON, one file per run with an ISO-style timestamp in the name so multiple runs are preserved.
