using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Storytime.Core.Models;

namespace Storytime.Core.Handlers.Agents {


  // --- Set Agent handlers. 

  public record AddPerformanceForCallSheetCommand(
    int CallSheetId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;

  
  public class AddPerformanceForCallSheetCommandHandler(StorytimeDbContext context)
      : IRequestHandler<AddPerformanceForCallSheetCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;

    public async Task<ItemDto?> Handle(AddPerformanceForCallSheetCommand request, CancellationToken cancellationToken) {
      if (request.CallSheetId <= 0) return null;

      var callSheetExists = await _context.Items
          .AnyAsync(i => i.Id == request.CallSheetId && i.IsActive, cancellationToken);
      if (!callSheetExists)
        throw new Exception($"CallSheet with id {request.CallSheetId} not found");

      var performance = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Performance,
        Description = request.Description,
        Data = JsonSerializer.Serialize(new PerformanceScript()),
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(performance);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.CallSheetId,
          RelationTypeId = (int)StRelationType.Produces,
          RelatedItemId = performance.Id
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      // Return the new Performance DTO directly — caller gets .Id immediately
      var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == performance.Id && i.IsActive)
          .Include(i => i.ItemType)
          .Include(i => i.Relations).ThenInclude(r => r.RelatedItem)
          .Include(i => i.Relations).ThenInclude(r => r.RelationType)
          .Include(i => i.IncomingRelations).ThenInclude(r => r.Item)
          .Include(i => i.IncomingRelations).ThenInclude(r => r.RelationType)
          .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(true);
    }
  }

  public record AddNarrationToPerformanceCommand(
    int PerformanceId,
    string Section,
    string Text
  ) : IRequest<ItemDto?>;

  public class AddNarrationToPerformanceCommandHandler(StorytimeDbContext context)
      : IRequestHandler<AddNarrationToPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;

    public async Task<ItemDto?> Handle(AddNarrationToPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) return null;

      var performance = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);
      if (performance == null)
        throw new Exception($"Performance with id {request.PerformanceId} not found");

      var script = string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}"
          ? new PerformanceScript()
          : JsonSerializer.Deserialize<PerformanceScript>(performance.Data) ?? new PerformanceScript();

      var nextRank = script.Entries.Count + 1;

      script.Entries.Add(new PerformanceEntry {
        Rank = nextRank,
        Type = "Narration",
        CharacterId = null,
        CharacterName = "Narrator",
        Text = request.Text
      });

      performance.Data = JsonSerializer.Serialize(script);
      await _context.SaveChangesAsync(cancellationToken);

      var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == request.PerformanceId && i.IsActive)
          .Include(i => i.ItemType)
          .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(false);
    }
  }

  // ─── Character Speak (MCP exposed) ───────────────────────────────────────────

  public record AddCharacterSpeakToPerformanceCommand(
    int PerformanceId,
    int CharacterId,
    string CharacterName,
    string Line
  ) : IRequest<ItemDto?>;

  public class AddCharacterSpeakToPerformanceCommandHandler(StorytimeDbContext context)
      : IRequestHandler<AddCharacterSpeakToPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;

    public async Task<ItemDto?> Handle(AddCharacterSpeakToPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) return null;

      var performance = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);
      if (performance == null)
        throw new Exception($"Performance with id {request.PerformanceId} not found");

      var script = string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}"
          ? new PerformanceScript()
          : JsonSerializer.Deserialize<PerformanceScript>(performance.Data) ?? new PerformanceScript();

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        var nextRank = await _context.ItemRelations
            .Where(ir => ir.ItemId == request.PerformanceId)
            .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.PerformanceId,
          RelationTypeId = (int)StRelationType.HasRole,
          RelatedItemId = request.CharacterId,
          Rank = nextRank
        });

        script.Entries.Add(new PerformanceEntry {
          Rank = nextRank,
          Type = "Speech",
          CharacterId = request.CharacterId,
          CharacterName = request.CharacterName,
          Text = request.Line
        });

        performance.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      // Return lightweight confirmation — just the new entry, not the whole performance
      var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == request.PerformanceId && i.IsActive)
          .Include(i => i.ItemType)
          .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(false);
    }
  }

  // ─── Character Action (MCP exposed) ──────────────────────────────────────────

  public record AddCharacterActionToPerformanceCommand(
    int PerformanceId,
    int CharacterId,
    string CharacterName,
    string Action
  ) : IRequest<ItemDto?>;

  public class AddCharacterActionToPerformanceCommandHandler(StorytimeDbContext context)
      : IRequestHandler<AddCharacterActionToPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;

    public async Task<ItemDto?> Handle(AddCharacterActionToPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) return null;

      var performance = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);
      if (performance == null)
        throw new Exception($"Performance with id {request.PerformanceId} not found");

      var script = string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}"
          ? new PerformanceScript()
          : JsonSerializer.Deserialize<PerformanceScript>(performance.Data) ?? new PerformanceScript();

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        var nextRank = await _context.ItemRelations
            .Where(ir => ir.ItemId == request.PerformanceId)
            .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.PerformanceId,
          RelationTypeId = (int)StRelationType.HasRole,
          RelatedItemId = request.CharacterId,
          Rank = nextRank
        });

        script.Entries.Add(new PerformanceEntry {
          Rank = nextRank,
          Type = "Action",
          CharacterId = request.CharacterId,
          CharacterName = request.CharacterName,
          Text = request.Action
        });

        performance.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == request.PerformanceId && i.IsActive)
          .Include(i => i.ItemType)
          .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(false);
    }
  }

  public record AddDeliverableForPerformanceCommand(
  int PerformanceId,
  string Name,
  string Content
) : IRequest<ItemDto?>;

  public class AddDeliverableForPerformanceCommandHandler(StorytimeDbContext context)
      : IRequestHandler<AddDeliverableForPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;

    public async Task<ItemDto?> Handle(AddDeliverableForPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) return null;

      var performanceExists = await _context.Items
          .AnyAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);
      if (!performanceExists)
        throw new Exception($"Performance with id {request.PerformanceId} not found");

      var deliverable = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Deliverable,
        Description = request.Content,   // prose lives in Description — human readable, no JSON
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(deliverable);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.PerformanceId,
          RelationTypeId = (int)StRelationType.Produces,
          RelatedItemId = deliverable.Id
        });

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == deliverable.Id && i.IsActive)
          .Include(i => i.ItemType)
          .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(false);
    }
  }

}
