using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core.Handlers.ItemRelations {
  public record CreateItemRelationCommand(
      int ItemId,
      int RelationTypeId,
      int RelatedItemId
  ) : IRequest<ItemRelationDto?>;

  public class CreateItemRelationCommandHandler : IRequestHandler<CreateItemRelationCommand, ItemRelationDto?> {
    private readonly StorytimeDbContext _context;
    public CreateItemRelationCommandHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<ItemRelationDto?> Handle(CreateItemRelationCommand request, CancellationToken cancellationToken) {
      var itemRelation = new ItemRelation {
        ItemId = request.ItemId,
        RelationTypeId = request.RelationTypeId,
        RelatedItemId = request.RelatedItemId
      };
      _context.ItemRelations.Add(itemRelation);
      await _context.SaveChangesAsync(cancellationToken);

      var query = _context.ItemRelations
       .Include(ir => ir.Item)
       .Include(ir => ir.RelatedItem)
       .Include(ir => ir.RelationType)
       .AsNoTracking()
       .AsQueryable();
      query = query.Where(ir => ir.Id == itemRelation.Id);
      var result = await query.FirstOrDefaultAsync(cancellationToken);
      return result?.ToDto();

    }
  }
}
