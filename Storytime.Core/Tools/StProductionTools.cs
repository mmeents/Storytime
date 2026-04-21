using Storytime.Core.Constants;
using MCPSharp;
using Storytime.Core.Service;
using System.ComponentModel;

namespace Storytime.Core.Tools {  

  public class StProductionTools {
    private static IStProductionToolsHandler GetTools() => DiBridgeService.GetService<IStProductionToolsHandler>();

    [McpTool(Cx.CmdAddCharacterAction, "Adds a new character action to a performance.")]
    public static async Task<string> AddCharacterActionToPerformance(
      [Description("Id of the performance item")]
      int performanceId,
      [Description("Id of the character")]
      int characterId,
      [Description("Name of the character")]
      string characterName,
      [Description("Describe the action performed by the character")]
      string action 
    ) => await GetTools().AddCharacterActionToPerformance(performanceId, characterId, characterName, action);

    [McpTool(Cx.CmdAddCharacterSpeak, "Adds a new line of dialogue for a character in a performance.")]
    public static async Task<string> AddCharacterSpeakToPerformance(
      [Description("Id of the performance item")]
      int performanceId,
      [Description("Id of the character")]
      int characterId,
      [Description("Name of the character")]
      string characterName,
      [Description("Line spoken by the character")]
      string line 
    ) => await GetTools().AddCharacterSpeakToPerformance(performanceId, characterId, characterName, line);

  }
}
