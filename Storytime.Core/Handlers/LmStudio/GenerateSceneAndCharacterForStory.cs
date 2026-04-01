using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Agents;

namespace Storytime.Core.Handlers.LmStudio {
  public record GenerateSceneAndCharacterForStoryCommand(
    int StoryId    
  ) : IRequest<bool>;

  internal class GenerateSceneAndCharacterForStoryHandler : IRequestHandler<GenerateSceneAndCharacterForStoryCommand, bool> {
    private readonly IStoryWriterAgent _storyWriterAgent;

    public GenerateSceneAndCharacterForStoryHandler(IStoryWriterAgent storyWriterAgent) {
      _storyWriterAgent = storyWriterAgent;
    }

    public async Task<bool> Handle(GenerateSceneAndCharacterForStoryCommand request, CancellationToken cancellationToken) {
      await _storyWriterAgent.GenerateSceneAndCharacterForStory(request.StoryId, cancellationToken);
      return true;
    }
  }
}
