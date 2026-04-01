using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Models;
using System.Text.Json;


namespace Storytime.Core.Handlers.Agents {

  public record AddCallSheetToSceneCommand(
    int SceneId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;

  // This returns the CallSheet since it's only being used internally and not by a mcp server.
  public class AddCallSheetToSceneCommandHandler(StorytimeDbContext context) : IRequestHandler<AddCallSheetToSceneCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddCallSheetToSceneCommand request, CancellationToken cancellationToken) {
      if (request.SceneId <= 0) return null;
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.SceneId && i.IsActive, cancellationToken);
      if (!parentExists) throw new Exception($"Parent item with id {request.SceneId} not found");
      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.CallSheet,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };
      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);
        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.SceneId)
          .CountAsync(cancellationToken) + 1;
        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.SceneId,
          RelationTypeId = (int)StRelationType.DirectedAs,
          RelatedItemId = newRelatedItem.Id
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }
      var result = await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == newRelatedItem.Id && i.IsActive)
        .Include(i => i.ItemType)
        .Include(i => i.Relations).ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations).ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations).ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations).ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(true);
    }
  }

  public record AddRoleToCallSheetCommand(
    int CallSheetId,
    int CharacterId,
    string Name,
    string Instruction
  ) : IRequest<ItemDto?>;

  public class AddRoleToCallSheetCommandHandler(StorytimeDbContext context) : IRequestHandler<AddRoleToCallSheetCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddRoleToCallSheetCommand request, CancellationToken cancellationToken) {

      if (request.CallSheetId <= 0) return null;
      if (request.CharacterId <= 0) return null;

      var callSheet = await _context.Items
        .FirstOrDefaultAsync(i => i.Id == request.CallSheetId && i.IsActive, cancellationToken);
      if (callSheet == null) throw new Exception($"Parent item with id {request.CallSheetId} not found");

      var script = string.IsNullOrWhiteSpace(callSheet.Data) || callSheet.Data == "{}"
        ? new CallSheetScript()
        : JsonSerializer.Deserialize<CallSheetScript>(callSheet.Data) ?? new CallSheetScript();

      var characterExists = await _context.Items.AnyAsync(i => i.Id == request.CharacterId && i.IsActive, cancellationToken);
      if (!characterExists) throw new Exception($"Character item with id {request.CharacterId} not found");

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {

        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.CallSheetId)
          .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.CallSheetId,
          RelationTypeId = (int)StRelationType.HasRole,
          RelatedItemId = request.CharacterId,
          Rank = nextRank
        });

        script.Script.Add(new CharacterPrompt {
          Rank = nextRank,
          CharacterId = request.CharacterId,
          Name = request.Name,
          Instruction = request.Instruction
        });

        callSheet.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      var result = await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == request.CallSheetId && i.IsActive)
        .Include(i => i.ItemType)
        .Include(i => i.Relations).ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations).ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations).ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations).ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(true);
    }
  }


  public record AddNarrationToCallSheetCommand(
    int CallSheetId,
    string Section,
    string Narration
  ) : IRequest<ItemDto?>;

  public class AddNarrationToCallSheetCommandHandler(StorytimeDbContext context) : IRequestHandler<AddNarrationToCallSheetCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddNarrationToCallSheetCommand request, CancellationToken cancellationToken) {
    
      if (request.CallSheetId <= 0) return null;
    
      var callSheet = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.CallSheetId && i.IsActive, cancellationToken);
      if (callSheet == null) throw new Exception($"Parent item with id {request.CallSheetId} not found");
    
      var script = string.IsNullOrWhiteSpace(callSheet.Data) || callSheet.Data == "{}"
          ? new CallSheetScript()
          : JsonSerializer.Deserialize<CallSheetScript>(callSheet.Data) ?? new CallSheetScript();

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {

        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.CallSheetId)
          .CountAsync(cancellationToken) + 1;

        var newRelatedItem = new Item {
          Name = request.Section,
          ItemTypeId = (int)StItemType.Narration,
          Description = request.Narration,
          Data = "{}",
          IsActive = true
        };
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.CallSheetId,
          RelationTypeId = (int)StRelationType.Narrates,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });

        script.Script.Add(new CharacterPrompt {
          Rank = nextRank,    
          Type = "Narration",
          Name = request.Section,
          Instruction = request.Narration
        });

        callSheet.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      var result = await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == request.CallSheetId && i.IsActive)
        .Include(i => i.ItemType)
        .Include(i => i.Relations).ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations).ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations).ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations).ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken);
    
      return result?.ToDto(true);
    }
  }




}
