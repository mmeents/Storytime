using MediatR;
using KB.Core.Models;

namespace Storytime.Core.Handlers.ItemRelationTypes {
  public record UpdateItemRelationTypeCommand(int Id, string Relation, string Description) : IRequest<ItemRelationTypeDto>;

  public class UpdateItemRelationTypeCommandHandler : IRequestHandler<UpdateItemRelationTypeCommand, ItemRelationTypeDto> {
    private readonly StorytimeDbContext _context;
    public UpdateItemRelationTypeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ItemRelationTypeDto> Handle(UpdateItemRelationTypeCommand request, CancellationToken cancellationToken) {
      var itemRelationType = await _context.ItemRelationTypes.FindAsync(request.Id);
      if (itemRelationType == null) {
        throw new KeyNotFoundException($"ItemRelationType with Id {request.Id} not found.");
      }

      itemRelationType.Relation = request.Relation;
      itemRelationType.Description = request.Description;

      await _context.SaveChangesAsync(cancellationToken);

      return itemRelationType.ToDto();
    }
  }
}