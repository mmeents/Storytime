using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storytime.Core.Models;

namespace Storytime.Core.Handlers.Agents {
  public record AddStoryToProjectCommand(
    int ProjectId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;

  public class AddStoryToProjectCommandHandler(
    StorytimeDbContext context,
    ILogger<AddStoryToProjectCommandHandler> logger
  ) : IRequestHandler<AddStoryToProjectCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddStoryToProjectCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddStoryToProjectCommand request, CancellationToken cancellationToken) {
      if (request.ProjectId <= 0) { 
        _logger.LogError("Invalid ProjectId: {ProjectId}", request.ProjectId);
        return null; 
      }
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.ProjectId && i.IsActive, cancellationToken);
      if (!parentExists) {
        _logger.LogError("Parent item with id {ProjectId} not found", request.ProjectId);
        throw new Exception($"Parent item with id {request.ProjectId} not found");
      }
      
      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Story,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);

        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.ProjectId)
          .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.ProjectId,
          RelationTypeId = (int)StRelationType.Contains,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch (Exception ex) {
        _logger.LogError("Error adding story to project {ProjectId}: {ErrorMessage}", request.ProjectId, ex.Message);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }
      try { 
      return await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == newRelatedItem.Id && i.IsActive) 
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
        _logger.LogError("Error retrieving new story item {ItemId}: {ErrorMessage}", newRelatedItem.Id, ex.Message);
        throw;
      }
    }
  }



}   
