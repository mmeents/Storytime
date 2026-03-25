using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core.Handlers.Items {

  public record AddRelatedItemCommand(
    int ParentItemId,             // the thing we're attaching to
    string RelationType,        // "Contains", "HasBeat", "FeaturesCharacter" etc.
    string ItemTypeName,           // "Scene", "Beat", "Character" etc.
    string ItemName,
    string? ItemDescription = null, // 
    string? ItemDataJson = null     // the big JSON blob for actorPrompt, goal, beatType etc.        
  ) : IRequest<int>; // returns the new Item.Id

  public class AddRelatedItemCommandHandler : IRequestHandler<AddRelatedItemCommand, int> {
    private readonly StorytimeDbContext _context;
    public AddRelatedItemCommandHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<int> Handle(AddRelatedItemCommand request, CancellationToken cancellationToken) {

      var parentItem = await _context.Items.FindAsync(new object[] { request.ParentItemId }, cancellationToken);
      if (parentItem == null) {
        throw new Exception($"Parent item with id {request.ParentItemId} not found");
      }

      var itemTypeId = await _context.ItemTypes.Where(t => t.Name == request.ItemTypeName).Select(t => t.Id).FirstAsync(cancellationToken);
      if (itemTypeId == 0) {
        throw new Exception($"ItemType with name {request.ItemTypeName} not found");
      }

      var relationTypeId = await _context.ItemRelationTypes.Where(r => r.Relation == request.RelationType).Select(r => r.Id).FirstAsync(cancellationToken);
      if (relationTypeId == 0) {
        throw new Exception($"RelationType with name {request.RelationType} not found");
      }

      var newItem = new KB.Core.Entities.Item {
        Name = request.ItemName,
        Description = request.ItemDescription ?? "",
        Data = request.ItemDataJson ?? "{}",
        ItemTypeId = itemTypeId,
        IsActive = true
      };

      _context.Items.Add(newItem);

      await _context.SaveChangesAsync(cancellationToken);

      var itemRelation = new KB.Core.Entities.ItemRelation {
        ItemId = parentItem.Id,
        RelatedItemId = newItem.Id,
        RelationTypeId = relationTypeId
      };
      _context.ItemRelations.Add(itemRelation);
      await _context.SaveChangesAsync(cancellationToken);
      return newItem.Id;
    }
  }
}
