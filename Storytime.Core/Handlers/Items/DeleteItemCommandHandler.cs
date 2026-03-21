using MediatR;

namespace Storytime.Core.Handlers.Items {
  public record DeleteItemCommand(int Id) : IRequest<bool>;

  public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool> {
    private readonly StorytimeDbContext _context;
    public DeleteItemCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken) {
      var item = await _context.Items.FindAsync(request.Id);

      if (item == null) {
        throw new KeyNotFoundException("Item not found");
      }

      item.IsActive = false; // Soft delete: mark as inactive instead of removing from database

      _context.Items.Update(item);

      await _context.SaveChangesAsync(cancellationToken);

      return true;
    }
  }
}