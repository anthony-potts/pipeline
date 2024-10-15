using Pipeline.Pipelines.Models;
using Newtonsoft.Json;
using Pipeline.Interfaces;

namespace Pipeline.Pipelines.Steps;
public class SerializeStep : IPipelineStep<PersonPipelineContext>
{
    public PersonPipelineContext Process(PersonPipelineContext context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(SerializeStep),
            StartedAt = DateTime.UtcNow
        };

        PersonPipelineContext outputContext = new PersonPipelineContext
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
            string json = JsonConvert.SerializeObject(context.People);
            outputContext.OutputList = json;

            audit.WasSuccessful = true;
        }
        catch (Exception ex)
        {
            outputContext.IsSuccessful = false;
            outputContext.Errors.Add($"SerializeStep Exception: {ex.Message}");

            audit.WasSuccessful = false;
            audit.Errors.Add($"SerializeStep Exception: {ex.Message}");
        }
        finally
        {
            audit.EndedAt = DateTime.UtcNow;
            outputContext.StepAudits.Add(audit);
        }

        return outputContext;
    }
}