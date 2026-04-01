using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Agents;


namespace Storytime.Core.Handlers.LmStudio {
  public record GenerateDeliverableCommand(int PerformanceId) : IRequest<bool>;
  internal class GenerateDeliverableCommandHandler : IRequestHandler<GenerateDeliverableCommand, bool> {
    private readonly IObserverAgent _observerAgent;

    public GenerateDeliverableCommandHandler(IObserverAgent observerAgent) {
      _observerAgent = observerAgent;
    }

    public async Task<bool> Handle(GenerateDeliverableCommand request, CancellationToken cancellationToken) {
      var deliverable = await _observerAgent.ObservePerformance(request.PerformanceId, cancellationToken);
      return deliverable != null;
    }
  }
}