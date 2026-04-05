
using MediatR;
using Microsoft.Extensions.Logging;
using Storytime.Core.Agents;

namespace Storytime.Core.Handlers.LmStudio {
  public record GenerateBeatsForSceneCommand(
    int StoryId,
    int SceneId    
  ) : IRequest<bool>;

  public class GenerateBeatsForSceneCommandHandler : IRequestHandler<GenerateBeatsForSceneCommand, bool> {
    private readonly ISceneWriterAgent _sceneWriterAgent;
    private readonly ILogger<GenerateBeatsForSceneCommandHandler> _logger;
    public GenerateBeatsForSceneCommandHandler(ISceneWriterAgent sceneWriterAgent, ILogger<GenerateBeatsForSceneCommandHandler> logger) {
      _sceneWriterAgent = sceneWriterAgent;
      _logger = logger;
    }
    public async Task<bool> Handle(GenerateBeatsForSceneCommand request, CancellationToken cancellationToken) {
      try { 
        await _sceneWriterAgent.GenerateBeatsForScene(request.StoryId, request.SceneId, cancellationToken);
        return true;
      } catch (Exception ex) {
        _logger.LogError(ex, "Error generating beats for scene {SceneId} in story {StoryId}: {Message}", request.SceneId, request.StoryId, ex.Message);
        return false;
      }        
    }
  }
}
