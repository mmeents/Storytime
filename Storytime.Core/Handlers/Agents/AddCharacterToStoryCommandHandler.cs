using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Storytime.Core.Handlers.Agents {

  public record AddCharacterToStoryCommand(
      int StoryId,
      string Name,
      string Description
  ) : IRequest<ItemDto?>;

  public class AddCharacterToStoryCommandHandler(
    StorytimeDbContext context,
    ILogger<AddCharacterToStoryCommandHandler> logger
  ) : IRequestHandler<AddCharacterToStoryCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddCharacterToStoryCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddCharacterToStoryCommand request, CancellationToken cancellationToken) {
      if (request.StoryId <= 0) {
        _logger.LogError("Invalid StoryId: {StoryId}", request.StoryId);
        return null;
      }
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.StoryId && i.IsActive, cancellationToken);
      if (!parentExists) {
        _logger.LogError("Parent item with id {StoryId} not found", request.StoryId);
        throw new Exception($"Parent item with id {request.StoryId} not found");
      }

      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Character,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);
        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.StoryId)
          .CountAsync(cancellationToken) + 1;
        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.StoryId,
          RelationTypeId = (int)StRelationType.FeaturesCharacter,
          RelatedItemId = newRelatedItem.Id
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch (Exception ex) {
        _logger.LogError("Error adding character to story {StoryId}: {ErrorMessage}", request.StoryId, ex.Message);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }
      try { 
        return await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == newRelatedItem.Id && i.IsActive)  // ← parent, as you had it
          .Include(i => i.ItemType)
          .Include(i => i.Relations)
              .ThenInclude(r => r.RelatedItem)
          .Include(i => i.Relations)
              .ThenInclude(r => r.RelationType)
          .Include(i => i.IncomingRelations)
              .ThenInclude(r => r.Item)
          .Include(i => i.IncomingRelations)
              .ThenInclude(r => r.RelationType)
          .FirstOrDefaultAsync(cancellationToken)
          .ContinueWith(t => t.Result?.ToDto(true), cancellationToken);
      } catch (Exception ex) {
        _logger.LogError("Error retrieving new character item {ItemId}: {ErrorMessage}", newRelatedItem.Id, ex.Message);
        throw;
      }
    }

  }

}
