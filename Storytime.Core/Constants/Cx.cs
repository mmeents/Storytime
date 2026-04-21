using KB.Core.Entities;
using Storytime.Core.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Constants {
  public static class Cx {
    public static string AppName => "Storytime";
    public static string McpAppName => "StorytimeMCP";
    public static string AppVersion => "1.0.0";
    public const string ApiLocalPort = "44344";
    public const string ApiLocalhostUrl = $"https://localhost:{ApiLocalPort}";  // via iis express 
        
    public const string LMStudioUrl = "http://10.0.0.118:8669";
    public const string LMStudioApiKey = "sk-lm-njtLGuVe:Vcbn9IXvEghho3wt9TCx";
    public const string LMStudioStorytimeMcpToolName = "mcp/storytimemcp";
    public const string LMStudioDefaultModel = "nvidia/nemotron-3-nano-4b";
    public const string ClaudeDefaultModel = "sonnet";
    public const int DefaultLmStudioContextLength = 8000;

    // Agent runner modes LmStudio, ClaudeCode 
    // public const AgentRunnerMode DefaultAgentRunnerMode = AgentRunnerMode.ClaudeCode;
    public const AgentRunnerMode DefaultAgentRunnerMode = AgentRunnerMode.LmStudio;

    public const string ValidRelationTypes = "Relation types(id:type): 1:Contains, 4:UsesRule, 5:FeaturesCharacter, 6:TakesPlaceAt, 7:UsesTone, 8:DirectedAs, 9:Produces 10:HasRole;";
    public const string ValidItemTypes = "Item types(id:name): 1:Project, 2:Story, 3:Scene, 4:Beat, 5:Character, 6:Location, 7:Rule, 8:Tone, 9:CallSheet, 10:Performance, 11:Deliverable;";

    public const string CmdGetHelp = "help";

    public const string CmdGetProjects = "getProjects";
    public const string CmdGetById = "getItemById";
    public const string CmdGetSubgraph = "getSubgraph";
    
    public const string CmdAddProjectStory = "addProjectStory"; 

    public const string CmdAddStoryScene = "addStoryScene"; 
    public const string CmdAddStoryCharacter = "addStoryCharacter";

    public const string CmdAddSceneBeat = "addSceneBeat"; 

    // director
    public const string CmdAddCallSheetNarration = "addCallSheetNarration";
    public const string CmdAddCallSheetRole = "addCallSheetRole";

    // performance
    public const string CmdAddCharacterAction = "addPerformanceAction"; 
    public const string CmdAddCharacterSpeak = "addPerformanceLine"; 

    public const string CmdUpdateItem = "updateItem";

/*  // disabled 
    public const string CmdAddRelationItem = "create-related-item";
    public const string CmdAddItem = "create-item"; 
    public const string CmdGetRelationById = "get-relation-by-id";
    public const string CmdAddRelataion = "create-relation";
    public const string CmdUpdateRelation = "update-relation";  */


    public const string tvKbUnloadedNodeText = "...loading...";



    public static string AsString(this StItemType type) { 
        return type switch {
            StItemType.Project => "Project",
            StItemType.Story => "Story",
            StItemType.Scene => "Scene",
            StItemType.Beat => "Beat",
            StItemType.Character => "Character",
            StItemType.Location => "Location",
            StItemType.Rule => "Rule",
            StItemType.Tone => "Tone",
            StItemType.CallSheet => "CallSheet",
            StItemType.Performance => "Performance",
            StItemType.Deliverable => "Deliverable",
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected item type value: {type}")
        };
    }

    public static int AsInt(this string value) {
      return int.Parse(value);
    }

    public static char[] InvalidFileNameChars() => Path.GetInvalidFileNameChars()
      .Concat(MyInvalidList()).ToArray();
    public static char[] MyInvalidList() => " `~!@#$%^&*()_-+=[]{},.;'".ToCharArray();
    public static string UrlSafe(this string str) {
      return string.Concat(str.Split(Cx.InvalidFileNameChars()));
    }

    public static string CommonAppPath {
      get {
        string commonPath = Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
          Cx.AppName).ResolvePath();
        if (!Directory.Exists(commonPath)) {
          Directory.CreateDirectory(commonPath);
        }
        return commonPath;
      }
    }

    public static string LogsAppPath {
      get {
        string logsPath = Path.Combine(CommonAppPath, "logs").ResolvePath();
        if (!Directory.Exists(logsPath)) {
          Directory.CreateDirectory(logsPath);
        }
        return logsPath;
      }
    }

    public static string ClaudeExecutablePath {
      get {
        string claudePath = Path.Combine(CommonAppPath, "claude").ResolvePath();
        if (!Directory.Exists(claudePath)) Directory.CreateDirectory(claudePath);
        if (!File.Exists(Path.Combine(claudePath, ".mcp.json")) ) {
          StringBuilder sb = new StringBuilder();
          sb.Append($"{{\r\n\t\"mcpServers\": {{\r\n\t  \"storytime-mcp\": {{\r\n\t\t\"type\": \"stdio\",\r\n\t\t\"command\": \"{claudePath}\\\\StorytimeMCP.exe\",\r\n\t\t\"args\": []\r\n\t  }}\r\n\t}}\r\n}}");
          File.WriteAllText(Path.Combine(claudePath, ".mcp.json"), sb.ToString());
        }
        return claudePath;
      }
    } 

    public static string ExportPath {
      get {
        string exportPath = Path.Combine(CommonAppPath, "exports").ResolvePath();
        if (!Directory.Exists(exportPath)) {
          Directory.CreateDirectory(exportPath);
        }
        return exportPath;
      }
    }


    public static string ResolvePath(this string path) {
      if (!Path.IsPathRooted(path)) {
        return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));
      }
      return Path.GetFullPath(path);
    }

  }
}
