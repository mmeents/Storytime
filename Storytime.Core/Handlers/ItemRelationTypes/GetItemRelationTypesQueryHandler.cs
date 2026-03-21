using MediatR;
using KB.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core.Handlers.ItemRelationTypes {
  public record GetItemRelationTypesQuery(int? Id, string? Relation) : IRequest<List<ItemRelationTypeDto>>;
  public class GetItemRelationTypesQueryHandler : IRequestHandler<GetItemRelationTypesQuery, List<ItemRelationTypeDto>> {
    private readonly StorytimeDbContext _context;

    public GetItemRelationTypesQueryHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<List<ItemRelationTypeDto>> Handle(GetItemRelationTypesQuery request, CancellationToken cancellationToken) {
      var query = _context.ItemRelationTypes
        .AsNoTracking()
        .AsQueryable();

      if (request.Id.HasValue) {
        query = query.Where(irt => irt.Id == request.Id.Value);
      }

      if (!string.IsNullOrEmpty(request.Relation)) {
        query = query.Where(irt => irt.Relation.Contains(request.Relation));
      }
      query = query.OrderBy(irt => irt.Relation);

      var itemRelationTypes = await query.ToListAsync(cancellationToken);
      return itemRelationTypes.Select(irt => irt.ToDto()).ToList();
    }
  }
}