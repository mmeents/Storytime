using MediatR;
using KB.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core.Handlers.ItemTypes {
  public record GetItemTypesQuery(int? Id, string? Name, string? Description) : IRequest<IEnumerable<ItemTypeDto>>;

  public class GetItemTypesQueryHandler : IRequestHandler<GetItemTypesQuery, IEnumerable<ItemTypeDto>> {
    private readonly StorytimeDbContext _context;

    public GetItemTypesQueryHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<IEnumerable<ItemTypeDto>> Handle(GetItemTypesQuery request, CancellationToken cancellationToken) {
      var query = _context.ItemTypes.AsNoTracking().AsQueryable();

      if (request.Id.HasValue) {
        query = query.Where(it => it.Id == request.Id.Value);
      }
      if (!string.IsNullOrEmpty(request.Name)) {
        query = query.Where(it => it.Name.Contains(request.Name));
      }
      if (!string.IsNullOrEmpty(request.Description)) {
        query = query.Where(it => it.Description.Contains(request.Description));
      }

      query = query.OrderBy(it => it.Name);

      var itemTypes = await query.ToListAsync(cancellationToken);
      return itemTypes.Select(it => it.ToDto());
    }
  }
}
