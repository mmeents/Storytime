using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using KB.Core.Models;
using Storytime.Core.Agents;

namespace Storytime.Core.Handlers.LmStudio {

  public record GenerateStoryCommand(
    int ProjectId    
  ) : IRequest<bool>;


  public class GenerateStoryCommandHandler : IRequestHandler<GenerateStoryCommand, bool> {
    private readonly IDevelopmentManagerAgent _developmentManagerAgent;
    public GenerateStoryCommandHandler(IDevelopmentManagerAgent developmentManagerAgent) {
      _developmentManagerAgent = developmentManagerAgent;
    }

    public async Task<bool> Handle(GenerateStoryCommand request, CancellationToken cancellationToken) {
      await _developmentManagerAgent.GenerateStoryIdeaForProject(request.ProjectId, cancellationToken);
      return true;
    }
  }
}
