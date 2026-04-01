using Microsoft.Extensions.DependencyInjection;
using Storytime.Core.Handlers.Agents;
using Storytime.Core.Models;
using Storytime.Core.Constants;
using MediatR;
using System.Text.Json;

namespace Storytime.Core.Tools {
  public interface IStProductionToolsHandler {
    Task<string> AddCharacterSpeakToPerformance(int PerformanceId, int CharacterId, string CharacterName, string Line);
    Task<string> AddCharacterActionToPerformance(int PerformanceId, int CharacterId, string CharacterName, string ActionDescription);
  }
  public class StProductionToolsHandler(IServiceScopeFactory serviceScopeFactory) : IStProductionToolsHandler {
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async Task<string> AddCharacterActionToPerformance(int PerformanceId, int CharacterId, string CharacterName, string ActionDescription) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddCharacterActionToPerformanceCommand(PerformanceId, CharacterId, CharacterName, ActionDescription);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure(Cx.CmdAddCharacterAction, $"Failed to add action for character {CharacterName} in performance {PerformanceId}.");        
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess(Cx.CmdAddCharacterAction, $"Success.");
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> AddCharacterSpeakToPerformance(int PerformanceId, int CharacterId, string CharacterName, string Line) {
      using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new AddCharacterSpeakToPerformanceCommand(PerformanceId, CharacterId, CharacterName, Line);
        var result = await mediator.Send(command);
        if (result == null) {
          var opResult = McpOpResult.CreateFailure(Cx.CmdAddCharacterSpeak, $"Failed to add line for character {CharacterName} in performance {PerformanceId}.");        
          return JsonSerializer.Serialize(opResult);
        } else {
          var opResult = McpOpResult.CreateSuccess(Cx.CmdAddCharacterSpeak, $"Success.");
          return JsonSerializer.Serialize(opResult);
      }
    }
  }
}
