// using Pipeline.Interfaces;
// using Pipeline.Pipelines.OrderParserPipeline.Models;
//
// namespace Pipeline.Pipelines.OrderParserPipeline.Steps;
//
// public class ParseOrderStep : IPipelineStep<string, Order>
// {
//     public PipelineContext<Order> Process(PipelineContext<string> context)
//     {
//         var audit = new PipelineStepAudit
//         {
//             StepName = nameof(ParseOrderStep),
//             StartedAt = DateTime.UtcNow
//         };
//
//         PipelineContext<Order> outputContext = new PipelineContext<Order>
//         {
//             Metadata = new Dictionary<string, object>(context.Metadata),
//             IsSuccessful = context.IsSuccessful,
//             Errors = new List<string>(context.Errors),
//             CreatedAt = context.CreatedAt,
//             CorrelationId = context.CorrelationId,
//             StepAudits = new List<PipelineStepAudit>(context.StepAudits)
//         };
//
//         try
//         {
//             var parts = context.Data.Split(',');
//             if (parts.Length != 3)
//                 throw new FormatException("Input data must be in the format 'OrderID, Product, Quantity'.");
//
//             var order = new Order
//             {
//                 OrderID = parts[0].Trim(),
//                 Product = parts[1].Trim(),
//                 Quantity = int.Parse(parts[2].Trim())
//             };
//
//             outputContext.Data = order;
//             audit.WasSuccessful = true;
//         }
//         catch (Exception ex)
//         {
//             outputContext.IsSuccessful = false;
//             outputContext.Errors.Add($"ParseOrderStep Exception: {ex.Message}");
//
//             audit.WasSuccessful = false;
//             audit.Errors.Add($"ParseOrderStep Exception: {ex.Message}");
//         }
//         finally
//         {
//             audit.EndedAt = DateTime.UtcNow;
//             outputContext.StepAudits.Add(audit);
//         }
//
//         return outputContext;
//     }
// }