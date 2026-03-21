using MediatR;


namespace Storytime.Core.Handlers.ItemTypes {
  public record DeleteItemTypeCommand(int Id) : IRequest<bool>;
  public class DeleteItemTypeCommandHandler : IRequestHandler<DeleteItemTypeCommand, bool> {
    private readonly StorytimeDbContext _context;

    public DeleteItemTypeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<bool> Handle(DeleteItemTypeCommand request, CancellationToken cancellationToken) {
      var itemType = await _context.ItemTypes.FindAsync(request.Id);
      if (itemType == null) {
        throw new KeyNotFoundException($"ItemType with Id {request.Id} not found.");
      }
      if (itemType.Items != null && itemType.Items.Any()) {
        throw new InvalidOperationException("Cannot delete ItemType that has associated Items.");
      }

      _context.ItemTypes.Remove(itemType);
      await _context.SaveChangesAsync(cancellationToken);

      return true;
    }
  }
}