using KB.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Storytime.Core.Constants;
using Storytime.Core.Handlers.Items;
using System.Text;

namespace Storytime.Core.Handlers.Export {
  public record ExportItemCommand(int ItemId, bool exportChildren, string exportPath) : IRequest<ExportItemCommandResult>;
  public record ExportItemCommandResult(bool Success, string? Message = null);
  public class ExportItemCommandHandler(IMediator mediator, ILogger<ExportItemCommandHandler> logger) : IRequestHandler<ExportItemCommand, ExportItemCommandResult> {
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<ExportItemCommandHandler> _logger = logger ;
    public async Task<ExportItemCommandResult> Handle(ExportItemCommand request, CancellationToken cancellationToken) {

      try { 
        var itemId = request.ItemId;
        var item = await _mediator.Send(new GetItemByIdQuery(itemId, true));
        if (item != null) {
          var exportPath = request.exportPath;          
          var exportFileName = $"{item.ItemTypeName}{itemId}-{item.Name.UrlSafe()}.md";
          var exportFullPath = Path.Combine(exportPath, exportFileName);

          var sb = new StringBuilder();          
          sb.AppendLine($" ###### {Path.GetFileNameWithoutExtension( exportFileName)}  {item.Established:yyyy-MM-dd}");          
          sb.AppendLine("");
          sb.AppendLine("");
          // body
          sb.AppendLine($" # {item.Name}");
          sb.AppendLine();          
          sb.AppendLine($"{item.Description}");
          sb.AppendLine();
          if (!string.IsNullOrEmpty(item.Data) && item.Data.Trim().Length > 2) { 
            sb.AppendLine("```JSON");
            sb.AppendLine($"{item.Data}");
            sb.AppendLine("```");
          }

          // incoming relations
          if (item.IncomingRelations.Count > 0) {            
            sb.AppendLine("## Incoming Relations");
            foreach (ItemRelationDto child in item.IncomingRelations) {
              int relatedItemId = child?.ItemId ?? 0;
              if (relatedItemId != 0) {
                var aParent = await _mediator.Send(new GetItemByIdQuery(relatedItemId, true));
                if (aParent != null) {
                  var parentFileName = $"{aParent.ItemTypeName}{relatedItemId}-{aParent.Name.UrlSafe()}.md";
                  sb.AppendLine($" - [{aParent.Name}]({parentFileName}) - {aParent.ItemTypeName}");
                }
              }
            }
            sb.AppendLine();
          }

          // relations
          if (item.Relations.Count > 0) {            
            sb.AppendLine("## Relations");
            foreach (ItemRelationDto child in item.Relations) {
              int relatedItemId = child?.RelatedItemId ?? 0;
              if (relatedItemId != 0) {
                var aChild = await _mediator.Send(new GetItemByIdQuery(relatedItemId, true));
                if (aChild != null) {
                  var childFileName = $"{aChild.ItemTypeName}{relatedItemId}-{aChild.Name.UrlSafe()}.md";
                  sb.AppendLine($" - [{aChild.Name}]({childFileName}) - {aChild.ItemTypeName}");
                }
              }            
            }
          }
          await File.WriteAllTextAsync(exportFullPath, sb.ToString());

          if (request.exportChildren) {
            foreach (ItemRelationDto child in item.Relations) {
              int relatedItemId = child?.RelatedItemId ?? 0;
              if (relatedItemId != 0) {
                var aChild = await _mediator.Send(new ExportItemCommand(relatedItemId, true, exportPath));              
                if (aChild != null && !aChild.Success) { 
                  return new ExportItemCommandResult(false, $"Failed to export child item with ID {relatedItemId}: {aChild.Message}");                  
                }
              }
            }
          }

        }
        return new ExportItemCommandResult(true);
      } catch (Exception ex) {
        // log error
        _logger.LogError(ex, "Error exporting item");
        return new ExportItemCommandResult(false, ex.Message);
      }
    }
      
    
  }
}
