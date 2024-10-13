using Pipeline.Interfaces;
using Pipeline.Pipelines.Models;

namespace Pipeline.Pipelines.Steps;

public class ParseStep : IPipelineStep<string, Person>
{
    public PipelineContext<Person> Process(PipelineContext<string> context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(ParseStep),
            StartedAt = DateTime.UtcNow
        };

        PipelineContext<Person> outputContext = new PipelineContext<Person>
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
            var parts = context.Data.Split(',');
            if (parts.Length != 2)
                throw new FormatException("Input data must be in the format 'Name, Age'.");

            var person = new Person
            {
                Name = parts[0].Trim(),
                Age = int.Parse(parts[1].Trim())
            };

            outputContext.Data = person;
            audit.WasSuccessful = true;
        }
        catch (Exception ex)
        {
            outputContext.IsSuccessful = false;
            outputContext.Errors.Add($"ParseStep Exception: {ex.Message}");

            audit.WasSuccessful = false;
            audit.Errors.Add($"ParseStep Exception: {ex.Message}");
        }
        finally
        {
            audit.EndedAt = DateTime.UtcNow;
            outputContext.StepAudits.Add(audit);
        }

        return outputContext;
    }
}