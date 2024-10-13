using Newtonsoft.Json;
using Pipeline.Interfaces;
using Pipeline.Pipelines.OrderParserPipeline.Models;

namespace Pipeline.Pipelines.OrderParserPipeline.Steps;

public class SerializeOrderStep : IPipelineStep<Order, string>
{
    public PipelineContext<string> Process(PipelineContext<Order> context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(SerializeOrderStep),
            StartedAt = DateTime.UtcNow
        };

        PipelineContext<string> outputContext = new PipelineContext<string>
        {
            Metadata = new Dictionary<string, object>(context.Metadata),
            IsSuccessful = context.IsSuccessful,
            Errors = new List<string>(context.Errors),
            CreatedAt = context.CreatedAt,
            CorrelationId = context.CorrelationId,
            StepAudits = new List<PipelineStepAudit>(context.StepAudits)
        };

        try
        {
            string json = JsonConvert.SerializeObject(context.Data);
            outputContext.Data = json;

            audit.WasSuccessful = true;
        }
        catch (Exception ex)
        {
            outputContext.IsSuccessful = false;
            outputContext.Errors.Add($"SerializeOrderStep Exception: {ex.Message}");

            audit.WasSuccessful = false;
            audit.Errors.Add($"SerializeOrderStep Exception: {ex.Message}");
        }
        finally
        {
            audit.EndedAt = DateTime.UtcNow;
            outputContext.StepAudits.Add(audit);
        }

        return outputContext;
    }
}