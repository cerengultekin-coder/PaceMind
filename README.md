# PaceMind

**An adaptive, multi-sport training coach that fits the plan to you — not you to the plan.**

Most athletes choose between an expensive personal coach and a static PDF plan that ignores
how their week actually went. PaceMind sits in between: it builds a periodized plan from your
goal, then **rewrites the weeks ahead from your feedback** — and explains *why* it changed.

## How it works

1. **Set a goal** — e.g. "run 10K in 50 minutes in 12 weeks", plus the days you can train.
2. **Get a plan** — a periodized build (ramp → recovery weeks → taper), each session sized by
   training load. Only the first week is fixed; later weeks stay drafts.
3. **Give feedback** — after each session you log how it felt (and, later, upload activity data).
4. **It adapts** — the plan reshapes around your feedback and flags your goal if it's at risk.

The "brain" is **hybrid**: deterministic rules own the safe training math (load progression,
periodization), while an LLM layer (planned) handles explanations and coaching chat.

## Tech stack

- **.NET 10 / C#** with **Clean Architecture**
- **ASP.NET Core** minimal API
- **Blazor WebAssembly** front end (hosted by the API)
- **Tailwind CSS v4** for styling
- Sport-agnostic training engine using a common **training-load** model and pluggable
  **sport profiles** (Strategy pattern)

## Project structure

| Project | Responsibility |
| --- | --- |
| `PaceMind.Domain` | Entities and the training engine (sport profiles, training load, plan generator) |
| `PaceMind.Application` | Composition / dependency injection for the engine |
| `PaceMind.Infrastructure` | External concerns (persistence, integrations) — in progress |
| `PaceMind.Api` | Minimal API; also hosts the Blazor app |
| `PaceMind.Web` | Blazor WebAssembly UI |
| `PaceMind.Contracts` | DTOs shared between the API and the web client |

## Running locally

Requirements: **.NET 10 SDK** and **Node.js** (for the Tailwind build).

```bash
dotnet run --project src/PaceMind.Api
```

Then open the app at the URL printed in the console (e.g. `http://localhost:5297`).

## Status

Early stage. The plan generator and the full UI are working end to end. Next up: the weekly
adapter (feedback → rewritten plan), activity-data (GPX) upload, persistence, and the AI coach.

> Note: a single-sport (running) profile ships first; the architecture adds new sports by
> adding a profile, without changing the engine.
