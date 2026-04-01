using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Agents;

namespace Storytime.Core.Handlers.LmStudio {
  public record GenerateBeatsForSceneCommand(
    int StoryId,
    int SceneId    
  ) : IRequest<bool>;

  public class GenerateBeatsForSceneCommandHandler : IRequestHandler<GenerateBeatsForSceneCommand, bool> {
    private readonly ISceneWriterAgent _sceneWriterAgent;
    public GenerateBeatsForSceneCommandHandler(ISceneWriterAgent sceneWriterAgent) {
      _sceneWriterAgent = sceneWriterAgent;
    }
    public async Task<bool> Handle(GenerateBeatsForSceneCommand request, CancellationToken cancellationToken) {
      await _sceneWriterAgent.GenerateBeatsForScene(request.StoryId, request.SceneId, cancellationToken);
      return true;
    }
  }
}
