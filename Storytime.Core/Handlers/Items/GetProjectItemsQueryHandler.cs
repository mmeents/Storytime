
using MediatR;
using KB.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace Storytime.Core.Handlers.Items {
  public class GetProjectItemsQuery : IRequest<List<ItemDto>> {    
    public GetProjectItemsQuery() {}
  }
  public class GetProjectItemsQueryHandler : IRequestHandler<GetProjectItemsQuery, List<ItemDto>> {
    private readonly StorytimeDbContext _context;
    public GetProjectItemsQueryHandler(StorytimeDbContext context) {
      _context = context;
    }
    
    public async Task<List<ItemDto>> Handle(GetProjectItemsQuery request, CancellationToken cancellationToken) {
      

      var query = _context.Items
        .Where(i => i.ItemTypeId == (int)StItemType.Project && i.IsActive
          && !i.IncomingRelations.Any(r => r.Item.ItemTypeId == (int)StItemType.Project))
        .Include(i => i.ItemType)
            .Include(i => i.Relations)
                .ThenInclude(r => r.RelatedItem)
            .Include(i => i.Relations)
                .ThenInclude(r => r.RelationType)
            .Include(i => i.IncomingRelations)
                .ThenInclude(r => r.Item)
            .Include(i => i.IncomingRelations)
                .ThenInclude(r => r.RelationType)
        .OrderBy(i => i.Name).Select(i => i.ToDto(true));


      var resultList = await query.ToListAsync();
      return resultList;
    }
  }
}
