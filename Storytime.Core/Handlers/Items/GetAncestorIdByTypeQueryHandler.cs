using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core.Handlers.Items {

  /// <summary>
  /// Walks IncomingRelations upward from a given item to find the nearest
  /// ancestor of the requested type. Returns null if not found.
  /// Example: GetAncestorIdByTypeQuery(sceneId, StItemType.Story)
  ///          → returns the storyId that Contains this scene.
  /// </summary>
  public record GetAncestorIdByTypeQuery(int ItemId, StItemType AncestorType ) : IRequest<int?>;

  public class GetAncestorIdByTypeQueryHandler : IRequestHandler<GetAncestorIdByTypeQuery, int?> {
    private readonly StorytimeDbContext _context;
    public GetAncestorIdByTypeQueryHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<int?> Handle(GetAncestorIdByTypeQuery request, CancellationToken cancellationToken) {
      var currentId = request.ItemId;
      var targetTypeId = (int)request.AncestorType;

      // Walk up a maximum of 10 levels to avoid infinite loops
      for (int depth = 0; depth < 10; depth++) {
        // Find the parent via IncomingRelations — the item that has a relation pointing TO currentId
        var parent = await _context.ItemRelations
          .AsNoTracking()
          .Where(r => r.RelatedItemId == currentId)
          .Select(r => new { r.ItemId, r.Item.ItemTypeId })
          .FirstOrDefaultAsync(cancellationToken);

        if (parent == null)
          return null; // reached root with no match

        if (parent.ItemTypeId == targetTypeId)
          return parent.ItemId; // found it

        currentId = parent.ItemId; // keep walking up
      }

      return null; // exceeded max depth
    }
  }
}
