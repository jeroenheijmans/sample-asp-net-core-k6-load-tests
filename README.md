# Sample ASP.NET Core MVC application with k6 tests

Demonstration of k6 load tests for an ASP.NET Core MVC application.

> ⚠️ WARNING! This repository may be locked in time and not get further (security) updates. It was written as a one time demo.

## Using this repository

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) — to run the web application
- [k6](https://grafana.com/docs/k6/latest/set-up/install-k6/) — to run the load tests
- [Node.js + npm](https://nodejs.org/) — optional, for TypeScript intellisense in your editor

### 1. Start the web application

```powershell
cd SampleOrg.Foo/SampleOrg.Foo.Website
dotnet run
```

The app will be available at `https://localhost:7177`. The default login is **bart / secret**.

### 2. Run the load tests

```powershell
cd SampleOrg.Foo.LoadTests
```

Create a `config.local.json` file with the password (this file is gitignored):

```json
{
  "password": "secret"
}
```

Then run:

```powershell
k6 run BaseLoadTest.ts
```

See [`SampleOrg.Foo.LoadTests/README.md`](SampleOrg.Foo.LoadTests/README.md) for full details.
