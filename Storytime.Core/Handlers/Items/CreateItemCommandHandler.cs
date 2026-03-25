using MediatR;
using KB.Core.Entities;
using KB.Core.Models;

namespace Storytime.Core.Handlers.Items {

  public record CreateItemCommand(
    string Name,
    int ItemTypeId,
    string Description,
    string Data
  ) : IRequest<ItemDto>;


  public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto> {
    private readonly StorytimeDbContext _context;
    public CreateItemCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken) {

      var itemType = await _context.ItemTypes.FindAsync(new object[] { request.ItemTypeId }, cancellationToken);

      if (itemType == null) {
        throw new Exception($"ItemType with id {request.ItemTypeId} not found");
      }

      var item = new Item {
        Name = request.Name,
        ItemTypeId = itemType.Id,
        Description = request.Description,
        Data = request.Data,
        IsActive = true
      };

      _context.Items.Add(item);
      await _context.SaveChangesAsync(cancellationToken);

      return item.ToDto();
    }
  }
  
}