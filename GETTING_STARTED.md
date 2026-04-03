# Getting Started

Storytime is designed to be cloned and run locally. There is no installer or downloadable exe —
you'll need a development environment and a couple of supporting tools.

## Prerequisites

- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with .NET Desktop workload
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [LM Studio](https://lmstudio.ai/) — for local LLM inference
- [SQL Server or LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) — for the StorytimeDb
- Git

## Clone and Open

```bash
git clone https://github.com/mmeents/Storytime.git
cd Storytime
```

Open `Storytime.sln` in Visual Studio.

## Configuration

Most configuration lives in `Cx.cs`. Before running, review and update:

- **Connection string** — Please point it at your local SQL Server or LocalDB instance. The setting is in the appSetting.json files.  Each app has and needs one, Api, Mcp and App.
- **LM Studio endpoint** — default is set in Cs.cs and is configured for my setup.  You need to install it and find a good LLM. (I am using Nemotron from NVidia via LM Studio marketplace.) You will need to enable api usage and copy the url from that interface and update the Cx.cs. You will need to issue an api key from LM Studio and place it in the Cx.cs in order for MCP calls to work. It's best to verify MCP is installed via chat window locally before trying to ask to use via api.
- **Claude Code** — Install and launch a session from the Agent Launch Folder. Authenticate the session and use it to track your usage, and hold the session open. Be careful not to abuse your limits.
- **Export path** — folder where Markdown exports will be written
- **Model names** — match the models you have loaded in LM Studio

![screenshot1](images/LLMSettings.png)  

## Database Setup

In Visual Studio, open **Package Manager Console** and:

1. Set **Startup Project** to the API project
2. Set **Default Project** to `Storytime.Core`
3. Run:

```powershell
Update-Database -Context StorytimeDbContext
```

This will create the StorytimeDb schema — 4 main tables: `Item`, `ItemRelations`, `ItemTypes`, `ItemRelationTypes`; and a couple log tables `AgentLog` and `AgentQueueItem`.

## Run

Set the StorytimeApp project as startup and press **F5**. You will need to add the projects using right click menu.

## MCP Server 

Storytime integrates with StorytimeMCP server for agent orchestration:

- **storytime-mcp** — read access to the story graph

This is required in order for an agent to act.

## Need Help?

The codebase is designed to be explored with an AI assistant.  I would recommend using my other mcp tool DaemonsMCP to help inspect the code, similar mcp documentation in the wiki there. [daemons-mcp docs](https://github.com/mmeents/DaemonsMCP/wiki/V3-Installation)

Clone it, open it up, and ask Claude or another LLM to walk you through any part of it.

Have fun, and use the tickets to chat if you like or give feedback! This is an early release, mainly a learning excercise, and I would love to know what you think.  

Cheers Matt.
