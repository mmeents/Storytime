
using Storytime.Core.Handlers.Items;
using Storytime.Core.Handlers.ItemTypes;
using Storytime.Core.Handlers.ItemRelations;
using Storytime.Core.Handlers.ItemRelationTypes;
using Storytime.Core.Handlers.AsGraph;

using MediatR;

namespace Storytime.Api.Extensions {

  public static class ItemsEndpointsExt {

    public static WebApplication MapItemsEndpoints(this WebApplication app) {

      var group = app.MapGroup("/api/item").WithTags("Item");

      group.MapPost("/", async (CreateItemCommand command, IMediator mediator) => {
        var result = await mediator.Send(command);
        return Results.Created($"/api/item/{result?.Id??0}", result);
      }).WithName("CreateItem").WithDescription("Creates a new item.");

      group.MapGet("/{Id}-{IncludeRelations}", async (int Id, bool? IncludeRelations, IMediator mediator) => {
        var query = new GetItemByIdQuery(Id, IncludeRelations == null ? false : IncludeRelations.Value);
        var result = await mediator.Send(query);
        return Results.Ok(result);
      }).WithName("GetItems").WithDescription("Retrieves a list of items.");

      group.MapGet("/projects", async (IMediator mediator) => {
        var query = new GetProjectItemsQuery();
        var result = await mediator.Send(query);
        return Results.Ok(result);
      }).WithName("GetProjects").WithDescription("Retrieves a list of projects.");

      group.MapPut("/{Id}", async (int Id, UpdateItemCommand command, IMediator mediator) => {
        if (Id != command.Id) {
          return Results.BadRequest("ID in URL does not match ID in body.");
        }
        var result = await mediator.Send(command);
        return Results.Ok(result);
      }).WithName("UpdateItem").WithDescription("Updates an existing item.");

      group.MapDelete("/{Id}", async (int Id, IMediator mediator) => {
        var command = new DeleteItemCommand(Id);
        await mediator.Send(command);
        return Results.NoContent();
      }).WithName("DeleteItem").WithDescription("Deletes an item by ID.");

      group.MapGet("/", async ([AsParameters] GetSubgraphQuery query, IMediator mediator) => {
        var result = await mediator.Send(query);
        return Results.Ok(result);
      }).WithName("GetSubgraph").WithDescription("Gets the item and related out to depth levels deep.");

      return app;
    }

    public static WebApplication MapItemTypesEndpoints(this WebApplication app) {
      var group = app.MapGroup("/api/item-type").WithTags("ItemType");
      group.MapGet("/{Id}", async ([AsParameters] GetItemTypesQuery query, IMediator mediator) => {
        var result = await mediator.Send(query);
        return Results.Ok(result);
      }).WithName("GetItemType").WithDescription("Retrieves an item type by ID.");

      group.MapPost("/", async ([AsParameters] CreateItemTypeCommand command, IMediator mediator) => {
        var result = await mediator.Send(command);
        return Results.Created($"/api/item-type/{result.Id}", result);
      }).WithName("CreateItemType").WithDescription("Creates a new item type.");

      group.MapPut("/{Id}", async (int Id, UpdateItemTypeCommand command, IMediator mediator) => {
        if (Id != command.Id) {
          return Results.BadRequest("ID in URL does not match ID in body.");
        }
        var result = await mediator.Send(command);
        return Results.Ok(result);
      }).WithName("UpdateItemType").WithDescription("Updates an existing item type.");

      group.MapDelete("/{Id}", async (int Id, IMediator mediator) => {
        var command = new DeleteItemTypeCommand(Id);
        await mediator.Send(command);
        return Results.NoContent();
      }).WithName("DeleteItemType").WithDescription("Deletes an item type by ID.");
      return app;
    }

    public static WebApplication MapItemRelationsEndpoints(this WebApplication app) {
      var group = app.MapGroup("/api/item-relation").WithTags("ItemRelation");

      group.MapGet("/", async ([AsParameters] GetItemRelationsQuery query, IMediator mediator) => {
        var result = await mediator.Send(query);
        return Results.Ok(result);
      }).WithName("GetItemRelation").WithDescription("Retrieves items via filters.");

      group.MapGet("/{Id}", async (int Id, IMediator mediator) => {
        var query = new GetItemRelationsQuery(Id, ItemId: null, ToItemId: null, RelationTypeId: null);
        var result = await mediator.Send(query);
        if (result == null || !result.Any()) {
          return Results.NotFound();
        }
        return Results.Ok(result.First());
      }).WithName("GetItemRelationById").WithDescription("Retrieves an item relation by ID.");

      group.MapPost("/", async (CreateItemRelationCommand command, IMediator mediator) => {
        var result = await mediator.Send(command);
        return Results.Created($"/api/item-relation/{result.Id}", result);
      }).WithName("CreateItemRelation").WithDescription("Creates a new item relation.");

      group.MapPut("/{Id}", async (int Id, UpdateItemRelationCommand command, IMediator mediator) => {
        if (Id != command.Id) {
          return Results.BadRequest("ID in URL does not match ID in body.");
        }
        var result = await mediator.Send(command);
        return Results.Ok(result);
      }).WithName("UpdateItemRelation").WithDescription("Updates an existing item relation.");

      group.MapDelete("/{Id}", async (int Id, IMediator mediator) => {
        var command = new DeleteItemRelationCommand(Id);
        await mediator.Send(command);
        return Results.NoContent();
      }).WithName("DeleteItemRelation").WithDescription("Deletes an item relation by ID.");

      return app;
    }

    public static WebApplication MapItemRelationTypesEndpoints(this WebApplication app) {
      var group = app.MapGroup("/api/item-relation-type").WithTags("ItemRelationType");

      group.MapGet("/", async ([AsParameters] GetItemRelationTypesQuery query, IMediator mediator) => {
        var result = await mediator.Send(query);
        return Results.Ok(result);
      }).WithName("GetItemRelationTypes").WithDescription("Retrieves an item relation types via filters.");

      group.MapPost("/", async ([AsParameters] CreateItemRelationTypeCommand command, IMediator mediator) => {
        var result = await mediator.Send(command);
        return Results.Created($"/api/item-relation-type/{result.Id}", result);
      }).WithName("CreateItemRelationType").WithDescription("Creates a new item relation type.");

      group.MapGet("/{id:int}", async (int id, IMediator mediator) => {
        var query = new GetItemRelationTypesQuery(Id: id, Relation: null);
        var result = await mediator.Send(query);
        if (result == null || !result.Any()) {
          return Results.NotFound();
        }
        return Results.Ok(result.First());
      })
      .WithName("GetItemRelationTypeById")
      .WithDescription("Retrieves a single item type by ID.");

      group.MapPut("/{Id}", async (int Id, UpdateItemRelationTypeCommand command, IMediator mediator) => {
        if (Id != command.Id) {
          return Results.BadRequest("ID in URL does not match ID in body.");
        }
        var result = await mediator.Send(command);
        return Results.Ok(result);
      }).WithName("UpdateItemRelationType").WithDescription("Updates an existing item relation type.");

      group.MapDelete("/{Id}", async (int Id, IMediator mediator) => {
        var command = new DeleteItemRelationTypeCommand(Id);
        await mediator.Send(command);
        return Results.NoContent();
      }).WithName("DeleteItemRelationType").WithDescription("Deletes an item relation type by ID.");

      return app;
    }

  }




}
