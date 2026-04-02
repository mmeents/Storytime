using MediatR;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Entities;
using Storytime.Core.Handlers.Items;
using Storytime.Core.Handlers.LmStudio;
using Storytime.Core.Agents;

namespace Storytime.Core.Handlers.Queue {

  public record ProcessNextItemCommand() : IRequest<bool>;

  public class ProcessNextItemCommandHandler : IRequestHandler<ProcessNextItemCommand, bool> {
    private readonly IMediator _mediator;
    public ProcessNextItemCommandHandler(IMediator mediator) {
      _mediator = mediator;
    }

    public async Task<bool> Handle(ProcessNextItemCommand request, CancellationToken cancellationToken) {
      var nextItem = await _mediator.Send(new GetNextAgentQueueItemQuery(), cancellationToken);
      if (nextItem == null)
        return false;

      await _mediator.Send(new UpdateAgentQueueItemStatusCommand(nextItem.Id, AgentQueueStatus.Running), cancellationToken);
      try {
        await RunPipelineAsync(nextItem.ItemId, nextItem.TargetDepth, cancellationToken);
        await _mediator.Send(new UpdateAgentQueueItemStatusCommand(nextItem.Id, AgentQueueStatus.Completed), cancellationToken);
        return true;
      } catch (Exception ex) {
        await _mediator.Send(new UpdateAgentQueueItemStatusCommand(nextItem.Id, AgentQueueStatus.Failed, ex.Message), cancellationToken);
        return false;
      }
    }

    public async Task<bool> RunPipelineAsync(int itemId, StItemType TargetDepth, CancellationToken cancellationToken) {

      var item = await _mediator.Send(new GetItemByIdQuery(itemId, true), cancellationToken);
      if (item == null) return false;

      int storyId = 0;
      var Started = DateTime.UtcNow;
      var workingTypeId = item.ItemTypeId;
      var workingId = item.Id;
      var hasBeats = false;

      if (item.ItemTypeId == (int)StItemType.Scene) {
        hasBeats = item.Relations.Any(r => r.RelationTypeId == (int)StRelationType.Contains);
      }

      // Pre-resolve storyId for any item deeper than Story
      if (workingTypeId != (int)StItemType.Project && workingTypeId != (int)StItemType.Story) {
        var aStoryId = await _mediator.Send(new GetAncestorIdByTypeQuery(workingId, StItemType.Story), cancellationToken);
        if (aStoryId == null) return false;
        storyId = aStoryId.Value;
      }

      // ── Project → Story ──────────────────────────────────────────────────
      if (workingTypeId == (int)StItemType.Project) {
        await _mediator.Send(new GenerateStoryCommand(workingId), cancellationToken);
        item = await _mediator.Send(new GetItemByIdQuery(itemId, true), cancellationToken);
        if (item == null) return false;
        var nextItem = item.Relations
          .Where(r => r.RelationTypeId == (int)StRelationType.Contains && r.Established > Started)
          .OrderByDescending(r => r.Established).ToList();
        if (nextItem.Count == 0) return false;
        workingId = nextItem[0].RelatedItemId!.Value;
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        workingTypeId = item.ItemTypeId;
      }

      // ── Story → Scene ─────────────────────────────────────────────────────
      if (workingTypeId <= (int)TargetDepth && workingTypeId == (int)StItemType.Story) {
        storyId = workingId;
        await _mediator.Send(new GenerateSceneAndCharacterForStoryCommand(workingId), cancellationToken);
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        var nextItem = item.Relations
          .Where(r => r.RelationTypeId == (int)StRelationType.Contains && r.Established > Started)
          .OrderByDescending(r => r.Established).ToList();
        if (nextItem.Count == 0) return false;
        workingId = nextItem[0].RelatedItemId!.Value;  // sceneId
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        workingTypeId = item.ItemTypeId; // Scene        
      }

      // ── Scene → Beats ─────────────────────────────────────────────────────
      if (workingTypeId <= (int)TargetDepth && workingTypeId == (int)StItemType.Scene) {
        if (!hasBeats) {
          await _mediator.Send(new GenerateBeatsForSceneCommand(storyId, workingId), cancellationToken);
        }
        // advance regardless — menu entry is always on scene, beats live under it
        workingTypeId = (int)StItemType.Beat;
      }

      // ── Beats → CallSheet ─────────────────────────────────────────────────
      if (workingTypeId <= (int)TargetDepth && workingTypeId == (int)StItemType.Beat) {
        await _mediator.Send(new GenerateCallSheetCommand(storyId, workingId), cancellationToken);
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        var nextItem = item.Relations
          .Where(r => r.RelationTypeId == (int)StRelationType.DirectedAs && r.Established > Started)
          .OrderByDescending(r => r.Established).ToList();
        if (nextItem.Count == 0) return false;
        workingId = nextItem[0].RelatedItemId!.Value; // callSheetId
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        workingTypeId = item.ItemTypeId; // CallSheet
      }

      // ── CallSheet → Performance ───────────────────────────────────────────
      if (workingTypeId <= (int)TargetDepth && workingTypeId == (int)StItemType.CallSheet) {
        await _mediator.Send(new GeneratePerformanceForCallSheetCommand(storyId, workingId), cancellationToken);
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        var nextItem = item.Relations
          .Where(r => r.RelationTypeId == (int)StRelationType.Produces && r.Established > Started)
          .OrderByDescending(r => r.Established).ToList();
        if (nextItem.Count == 0) return false;
        workingId = nextItem[0].RelatedItemId!.Value; // performanceId
        item = await _mediator.Send(new GetItemByIdQuery(workingId, true), cancellationToken);
        if (item == null) return false;
        workingTypeId = item.ItemTypeId; // Performance
      }

      // ── Performance → Deliverable ─────────────────────────────────────────
      if (workingTypeId <= (int)TargetDepth && workingTypeId == (int)StItemType.Performance) {
        await _mediator.Send(new GenerateDeliverableCommand(workingId), cancellationToken);
      }

      return true;
    }
  }
}
