
using MediatR;
using KB.Core.Models;

namespace Storytime.Core.Handlers.ItemTypes {

  public record UpdateItemTypeCommand(
    int Id,
    string Name,
    string Description
  ) : IRequest<ItemTypeDto>;


  public class UpdateItemTypeCommandHandler : IRequestHandler<UpdateItemTypeCommand, ItemTypeDto> {
    private readonly StorytimeDbContext _context;

    public UpdateItemTypeCommandHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ItemTypeDto> Handle(UpdateItemTypeCommand request, CancellationToken cancellationToken) {
      var itemType = await _context.ItemTypes.FindAsync(request.Id);
      if (itemType == null) {
        throw new KeyNotFoundException($"ItemType with Id {request.Id} not found.");
      }

      itemType.Name = request.Name;
      itemType.Description = request.Description;

      await _context.SaveChangesAsync(cancellationToken);

      return itemType.ToDto();
    }
  }
}