using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Clients;


namespace Storytime.Core.Handlers.LmStudio {
  public class GetModelsQuery : IRequest<List<string>> {
  }
  internal class GetModelsQueryHandler : IRequestHandler<GetModelsQuery, List<string>> {
    private readonly ILmStudioClient _lmStudioClient;

    public GetModelsQueryHandler(ILmStudioClient lmStudioClient) {
      _lmStudioClient = lmStudioClient;
    }

    public async Task<List<string>> Handle(GetModelsQuery request, CancellationToken cancellationToken) {
      var models = await _lmStudioClient.GetLlmModelsAsync(cancellationToken);
      return models.Select(m => m.Key).ToList();
    }


  }
}