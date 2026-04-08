using MediatR;
using KB.Core.Models;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Constants;

namespace Storytime.Core.Handlers.Items {
  
  public record GetItemByIdQuery(int Id, bool IncludeRelations = false) : IRequest<ItemDto?>;

  public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto?> {
    private readonly StorytimeDbContext _context;
    public GetItemByIdQueryHandler(StorytimeDbContext context) {
      _context = context;
    }

    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken) {
      if (request.IncludeRelations) {
        return await _context.GetItemDtoById(request.Id, cancellationToken);
      }
      return await _context.GetMinimalItemDtoById(request.Id, cancellationToken);
    }
  }
}