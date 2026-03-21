using MediatR;

namespace Storytime.Core.Handlers.ItemRelations {
  public record DeleteItemRelationCommand(int ItemRelationId) : IRequest<bool>;

  public class DeleteItemRelationCommandHandler : IRequestHandler<DeleteItemRelationCommand, bool> {
    private readonly StorytimeDbContext _context;
    public DeleteItemRelationCommandHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<bool> Handle(DeleteItemRelationCommand request, CancellationToken cancellationToken) {
      var itemRelation = await _context.ItemRelations.FindAsync(request.ItemRelationId);
      if (itemRelation == null) {
        throw new KeyNotFoundException("Item relation not found");
      }
      _context.ItemRelations.Remove(itemRelation);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }
}