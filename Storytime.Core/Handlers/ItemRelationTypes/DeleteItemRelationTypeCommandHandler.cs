using MediatR;

namespace Storytime.Core.Handlers.ItemRelationTypes {
  public record DeleteItemRelationTypeCommand(int Id) : IRequest<bool>;
  public class DeleteItemRelationTypeCommandHandler : IRequestHandler<DeleteItemRelationTypeCommand, bool> {
    private readonly StorytimeDbContext _context;

    public DeleteItemRelationTypeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<bool> Handle(DeleteItemRelationTypeCommand request, CancellationToken cancellationToken) {
      var itemRelationType = await _context.ItemRelationTypes.FindAsync(request.Id);
      if (itemRelationType == null) {
        throw new KeyNotFoundException($"ItemRelationType with Id {request.Id} not found.");
      }
      if (itemRelationType.ItemRelations != null && itemRelationType.ItemRelations.Any()) {
        throw new InvalidOperationException("Cannot delete ItemRelationType that has associated Relations.");
      }

      _context.ItemRelationTypes.Remove(itemRelationType);
      await _context.SaveChangesAsync(cancellationToken);

      return true;
    }
  }
}