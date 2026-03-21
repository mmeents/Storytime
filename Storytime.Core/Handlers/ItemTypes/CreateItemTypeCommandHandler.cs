using MediatR;
using KB.Core.Models;
using KB.Core.Entities;

namespace Storytime.Core.Handlers.ItemTypes {
  public record CreateItemTypeCommand(string Name, string Description) : IRequest<ItemTypeDto>;
  public class CreateItemTypeCommandHandler : IRequestHandler<CreateItemTypeCommand, ItemTypeDto> {
    private readonly StorytimeDbContext _context;

    public CreateItemTypeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ItemTypeDto> Handle(CreateItemTypeCommand request, CancellationToken cancellationToken) {
      var itemType = new ItemType {
        Name = request.Name,
        Description = request.Description
      };

      _context.ItemTypes.Add(itemType);
      await _context.SaveChangesAsync(cancellationToken);

      return itemType.ToDto();
    }
  }
}
