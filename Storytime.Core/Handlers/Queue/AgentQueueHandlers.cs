using MediatR;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Entities;
using Storytime.Core.Handlers.Items;
using Storytime.Core.Agents;
using Storytime.Core.Handlers.LmStudio;

namespace Storytime.Core.Handlers.Queue {


  //  StAgentService.DirectorAgent
  // ── Add ────────────────────────────────────────────────────────────────────

  public record AddAgentQueueItemCommand(
    int ItemId,
    StItemType TargetDepth
  ) : IRequest<AgentQueueItem>;

  public class AddAgentQueueItemCommandHandler : IRequestHandler<AddAgentQueueItemCommand, AgentQueueItem> {
    private readonly StorytimeDbContext _context;
    public AddAgentQueueItemCommandHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<AgentQueueItem> Handle(AddAgentQueueItemCommand request, CancellationToken cancellationToken) {
      var entry = new AgentQueueItem {
        ItemId = request.ItemId,
        TargetDepth = request.TargetDepth,
        Status = AgentQueueStatus.Pending,
        ScheduledAt = DateTime.UtcNow
      };
      _context.AgentQueue.Add(entry);
      await _context.SaveChangesAsync(cancellationToken);
      return entry;
    }
  }

  // ── Get Next Pending ───────────────────────────────────────────────────────

  public record GetNextAgentQueueItemQuery() : IRequest<AgentQueueItem?>;

  public class GetNextAgentQueueItemQueryHandler : IRequestHandler<GetNextAgentQueueItemQuery, AgentQueueItem?> {
    private readonly StorytimeDbContext _context;
    public GetNextAgentQueueItemQueryHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<AgentQueueItem?> Handle(GetNextAgentQueueItemQuery request, CancellationToken cancellationToken) {
      return await _context.AgentQueue
        .Where(q => q.Status == AgentQueueStatus.Pending)
        .OrderBy(q => q.ScheduledAt)
        .FirstOrDefaultAsync(cancellationToken);
    }
  }

  // ── Update Status ──────────────────────────────────────────────────────────

  public record UpdateAgentQueueItemStatusCommand(
    int Id,
    AgentQueueStatus Status,
    string? ErrorMessage = null
  ) : IRequest<AgentQueueItem?>;

  public class UpdateAgentQueueItemStatusCommandHandler : IRequestHandler<UpdateAgentQueueItemStatusCommand, AgentQueueItem?> {
    private readonly StorytimeDbContext _context;
    public UpdateAgentQueueItemStatusCommandHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<AgentQueueItem?> Handle(UpdateAgentQueueItemStatusCommand request, CancellationToken cancellationToken) {
      var entry = await _context.AgentQueue.FindAsync(new object[] { request.Id }, cancellationToken);
      if (entry == null) return null;
      entry.Status = request.Status;
      entry.ErrorMessage = request.ErrorMessage;
      if (request.Status == AgentQueueStatus.Running)
        entry.StartedAt = DateTime.UtcNow;
      if (request.Status == AgentQueueStatus.Completed || request.Status == AgentQueueStatus.Failed)
        entry.CompletedAt = DateTime.UtcNow;
      await _context.SaveChangesAsync(cancellationToken);
      return entry;
    }
  }

  // ── Get All (for Factory Floor display) ───────────────────────────────────

  public record GetAgentQueueQuery(AgentQueueStatus? StatusFilter = null) : IRequest<List<AgentQueueItem>>;

  public class GetAgentQueueQueryHandler : IRequestHandler<GetAgentQueueQuery, List<AgentQueueItem>> {
    private readonly StorytimeDbContext _context;
    public GetAgentQueueQueryHandler(StorytimeDbContext context) {
      _context = context;
    }
    public async Task<List<AgentQueueItem>> Handle(GetAgentQueueQuery request, CancellationToken cancellationToken) {
      var query = _context.AgentQueue.AsQueryable();
      if (request.StatusFilter.HasValue)
        query = query.Where(q => q.Status == request.StatusFilter.Value);
      return await query.OrderByDescending(q => q.ScheduledAt).ToListAsync(cancellationToken);
    }
  }



  
}