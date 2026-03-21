using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core.Handlers.Items {
  public record CreateProjectTreeCommand(string ClaudeJson) : IRequest<ProjectImportResult>;

  public record ProjectImportResult(long ProjectId, string Name, int ScenesCreated);


  public class CreateProjectTreeCommandHandler : IRequestHandler<CreateProjectTreeCommand, ProjectImportResult> {
    private readonly StorytimeDbContext _context;   // your derived context
    public CreateProjectTreeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ProjectImportResult> Handle(CreateProjectTreeCommand request, CancellationToken ct) {
      var tree = JsonSerializer.Deserialize<ClaudeProjectTree>(request.ClaudeJson,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

      // Phase 1: Create all Items and map tempId → real Id
      var tempToReal = new Dictionary<int, int>();

      foreach (var dto in tree.Items) {
        var itemTypeId = await GetItemTypeId(dto.ItemType);
        var data = dto.Data ?? new JsonElement();  // handle null case
        string description = data.GetProperty("description").GetString() ?? "";
        var item = new Item {
          ItemTypeId = itemTypeId,
          Name = dto.Name,
          Description = description,
          Data = JsonSerializer.Serialize(data)   // everything else goes in the JSON column
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync(ct);
        tempToReal[dto.TempId] = item.Id;
      }

      // Phase 2: Create Relations
      foreach (var r in tree.Relations) {
        _context.ItemRelations.Add(new ItemRelation {
          ItemId = tempToReal[r.FromTempId],
          RelatedItemId = tempToReal[r.ToTempId],
          RelationTypeId = await GetRelationTypeId(r.RelationType)
        });
      }

      await _context.SaveChangesAsync(ct);

      var projectId = tempToReal[tree.Items.First(i => i.ItemType == "Project").TempId];

      return new ProjectImportResult(projectId, tree.Items.First(i => i.ItemType == "Project").Name,
          tree.Items.Count(i => i.ItemType == "Scene"));
    }

    private async Task<int> GetItemTypeId(string name)
        => (await _context.ItemTypes.FirstAsync(t => t.Name == name)).Id;

    private async Task<int> GetRelationTypeId(string name)
        => (await _context.ItemRelationTypes.FirstAsync(t => t.Relation == name)).Id;
  }


  public record ClaudeProjectTree(
    List<ImportItemDto> Items,
    List<ImportRelationDto> Relations
  );
  public record ImportItemDto(int TempId, string ItemType, string Name, JsonElement? Data);
  public record ImportRelationDto(int FromTempId, int ToTempId, string RelationType);
}
