using MediatR;
using Storytime.Core.Agents;

namespace Storytime.Core.Handlers.LmStudio {
  public record GeneratePerformanceForCallSheetCommand(
    int StoryId,
    int CallSheetId
    
  ) : IRequest<bool>;

  public class GeneratePerformanceCommandHandler : IRequestHandler<GeneratePerformanceForCallSheetCommand, bool> {
    private readonly ISetAgent _setAgent;

    public GeneratePerformanceCommandHandler(ISetAgent setAgent) {
      _setAgent = setAgent;
    }

    public async Task<bool> Handle(GeneratePerformanceForCallSheetCommand request, CancellationToken cancellationToken) {
      await _setAgent.PerformScene(request.CallSheetId, request.StoryId, cancellationToken);
      return true;
    }
  }
}
