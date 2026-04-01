using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Agents;


namespace Storytime.Core.Handlers.LmStudio {
  public record GenerateCallSheetCommand(
    int StoryId,
    int SceneId        
  ) : IRequest<bool>;

  public class GenerateCallSheetCommandHandler : IRequestHandler<GenerateCallSheetCommand, bool> {
    private readonly IDirectorAgent _directorAgent;

    public GenerateCallSheetCommandHandler(IDirectorAgent directorAgent) {
      _directorAgent = directorAgent;
    }

    public async Task<bool> Handle(GenerateCallSheetCommand request, CancellationToken cancellationToken) {
      await _directorAgent.DirectScene(request.StoryId, request.SceneId, cancellationToken);
      return true;
    }
  }
}
