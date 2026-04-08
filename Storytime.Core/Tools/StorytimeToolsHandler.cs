using KB.Core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Storytime.Core.Handlers.Items;
using Storytime.Core.Handlers.ItemRelations;
using Storytime.Core.Handlers.AsGraph;
using Storytime.Core.Models;
using System.Text.Json;
using Storytime.Core.Constants;

namespace Storytime.Core.Tools {
  public interface IStorytimeToolsHandler {
    string GetHelpText();
    Task<string> GetProjectItems();
    Task<string> GetItemById(int id);
    Task<string> CreateRelatedItem(int parentItemId, int relationTypeId, int itemTypeId, string name, string description, string data);
    Task<string> CreateItem(string name, int itemTypeId, string description, string data);
    Task<string> UpdateItem(int id, string name, int itemTypeId, string description, string data);
    Task<string> GetRelationById(int id);
    Task<string> CreateRelation(int fromItemId, int toItemId, int relationTypeId);
    Task<string> UpdateRelation(int relationId, int fromItemId, int toItemId, int relationTypeId, int rank);
    Task<string> GetSubgraph(int itemId, int depth = 3);
  }


  public class StorytimeToolsHandler : IStorytimeToolsHandler {
    private IServiceScopeFactory _serviceScopeFactory;
    private ILogger<StorytimeToolsHandler> _logger;

    public StorytimeToolsHandler(IServiceScopeFactory serviceScopeFactory, ILogger<StorytimeToolsHandler> logger) {
      _serviceScopeFactory = serviceScopeFactory;
      _logger = logger;
    }

    // Storytime tools are spread across 3 main handlers.
    public string GetHelpText() {
      var helpText = new Dictionary<string, string> {
        { "Notes:", "use dashes never underline characters for commands. "},
        { Cx.CmdGetHelp, "was used to get this text" },

        { Cx.CmdGetProjects, "returns list of projects, root items only" },

        { Cx.CmdGetById, "item lookup, items are the nouns. all ids are type int" },
        { Cx.CmdGetSubgraph, "root and related items out to depth levels of items deep.  Parameters: int itemId, int depth" },
        
        { Cx.CmdAddStory, "Adds a story to a project. Parameters: int projectId, string name, string description." },

        { Cx.CmdAddScene, "Adds a scene to a story. Parameters: int storyId, string name, string description." },

        { Cx.CmdAddBeat, "Adds a beat to a scene. Parameters: int sceneId, string name, string description." },
        { Cx.CmdAddCharacter, "Adds a character to a story. Parameters: int storyId, string name, string description." },
                
        { Cx.CmdAddNarrationToCallSheet, "Adds a narration beat to a call sheet. Parameters: int callSheetId, string name, string description" },
        { Cx.CmdAddRoleToCallSheet, "Adds a character role to a call sheet. Parameters: int callSheetId, int characterId, string name, string description" },
        
        { Cx.CmdAddCharacterAction, "Adds an action to a performance for a character. Parameters: int performanceId, int characterId, string characterName, string action" },
        { Cx.CmdAddCharacterSpeak, "Adds a speak moment to a performance for a character. Parameters: int performanceId, int characterId, string characterName, string line" },
       
      };
      return JsonSerializer.Serialize(helpText);
    }

    public async Task<string> GetProjectItems() {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var query = new GetProjectItemsQuery();
        var result = await mediator.Send(query);
        var opResult = McpOpResult.CreateSuccess("get-project-items", "Successfully retrieved project items", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error retrieving project items");
        var opResult = McpOpResult.CreateFailure("get-project-items", "Failed to retrieve project items", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> GetItemById(int id) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var query = new GetItemByIdQuery(id, true);
        var result = await mediator.Send(query);
        var opResult = McpOpResult.CreateSuccess("get-item-by-id", "Successfully retrieved item", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error retrieving item by id");
        var opResult = McpOpResult.CreateFailure("get-item-by-id", "Failed to retrieve item by id", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> CreateRelatedItem(int parentItemId, int relationTypeId, int itemTypeId, string name, string description, string data) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new CreateRelatedItemCommand(parentItemId, relationTypeId, itemTypeId, name, description, data);
        var result = await mediator.Send(command);
        var opResult = McpOpResult.CreateSuccess("create-related-item", "Successfully created related item", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error creating related item");
        var opResult = McpOpResult.CreateFailure("create-related-item", "Failed to create related item", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> CreateItem(string name, int itemTypeId, string description, string data) {
      try { 
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new CreateItemCommand(name, itemTypeId, description, data);
        var result = await mediator.Send(command);
        var opResult = McpOpResult.CreateSuccess("create-item", "Successfully created", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error creating item");
        var opResult = McpOpResult.CreateFailure("create-item", "Failed to create item", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> UpdateItem(int id, string name, int itemTypeId, string description, string data) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new UpdateItemCommand(id, itemTypeId, name, description, data, true);
        var result = await mediator.Send(command);
        var opResult = McpOpResult.CreateSuccess("update-item", "Successfully updated", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error updating item");
        var opResult = McpOpResult.CreateFailure("update-item", "Failed to update item", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> GetRelationById(int id) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var query = new GetItemRelationsQuery(id, null, null, null);
        var result = await mediator.Send(query);
        var opResult = McpOpResult.CreateSuccess("get-relation-by-id", "Successfully retrieved relation", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error retrieving relation by id");
        var opResult = McpOpResult.CreateFailure("get-relation-by-id", "Failed to retrieve relation by id", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> CreateRelation(int fromItemId, int toItemId, int relationTypeId) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new CreateItemRelationCommand(fromItemId, relationTypeId, toItemId);
        var result = await mediator.Send(command);
        var opResult = McpOpResult.CreateSuccess("create-relation", "Successfully created relation", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error creating item relation");
        var opResult = McpOpResult.CreateFailure("create-relation", "Failed to create item relation", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> UpdateRelation(int relationId, int fromItemId, int toItemId, int relationTypeId, int rank) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new UpdateItemRelationCommand(relationId, fromItemId, relationTypeId, toItemId, rank);
        var result = await mediator.Send(command);
        var opResult = McpOpResult.CreateSuccess("update-relation", "Successfully updated relation", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error updating item relation");
        var opResult = McpOpResult.CreateFailure("update-relation", "Failed to update item relation", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> GetSubgraph(int itemId, int depth) {
      try {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var query = new GetSubgraphQuery(itemId, depth);
        var result = await mediator.Send(query);
        var opResult = McpOpResult.CreateSuccess("get-subgraph", "Successfully retrieved subgraph", result);
        return JsonSerializer.Serialize(opResult);
      } catch (Exception ex) {
        _logger.LogError(ex, "Error retrieving subgraph");
        var opResult = McpOpResult.CreateFailure("get-subgraph", "Failed to retrieve subgraph", ex);
        return JsonSerializer.Serialize(opResult);
      }
    }

  }
}
