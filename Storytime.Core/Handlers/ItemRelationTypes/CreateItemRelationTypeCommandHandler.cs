using MediatR;
using KB.Core.Models;
using KB.Core.Entities;

namespace Storytime.Core.Handlers.ItemRelationTypes {
    public record CreateItemRelationTypeCommand(string Relation, string Description) : IRequest<ItemRelationTypeDto>;

  public class CreateItemRelationTypeCommandHandler : IRequestHandler<CreateItemRelationTypeCommand, ItemRelationTypeDto> {
    private readonly StorytimeDbContext _context;
    public CreateItemRelationTypeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ItemRelationTypeDto> Handle(CreateItemRelationTypeCommand request, CancellationToken cancellationToken) {
      var itemRelationType = new ItemRelationType {
        Relation = request.Relation,
        Description = request.Description
      };

      _context.ItemRelationTypes.Add(itemRelationType);
      await _context.SaveChangesAsync(cancellationToken);

      return itemRelationType.ToDto();
    }
  }
}