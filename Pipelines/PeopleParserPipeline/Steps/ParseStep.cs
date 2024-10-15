using Pipeline.Interfaces;
using Pipeline.Pipelines.Models;

namespace Pipeline.Pipelines.Steps;

public class ParseStep : IPipelineStep<PersonPipelineContext>
{
    private readonly List<string> _inputData;
    public ParseStep(List<string> inputData)
    {
        _inputData = inputData;
    }


    public PersonPipelineContext Process(PersonPipelineContext context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(ParseStep),
            StartedAt = DateTime.UtcNow
        };

        PersonPipelineContext outputContext = new PersonPipelineContext()
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
            foreach (var input in _inputData)
            {
                var parts = input.Split(',');
                if (parts.Length != 2)
                {
                    audit.Errors.Add($"Input data must be in the format 'Name, Age'. Error Line: {input}");
                }
                else
                {
                    var person = new Person
                    {
                        Name = parts[0].Trim(),
                        Age = int.Parse(parts[1].Trim())
                    };
                           
                    outputContext.People.Add(person);
                   
                }
            }
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