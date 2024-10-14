using Pipeline.Interfaces;
using Pipeline.Pipelines.Models;

namespace Pipeline.Pipelines.Steps;

using System.Collections.Generic;
using System.Linq;

public class ValidateStep : IPipelineStep<Person, Person>
{
    public PipelineContext<Person> Process(PipelineContext<Person> context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(ValidateStep),
            StartedAt = System.DateTime.UtcNow
        };

        var errors = new List<string>();

        if (string.IsNullOrEmpty(context.Data.Name))
            errors.Add("Name cannot be empty.");
        if (context.Data.Age < 0 || context.Data.Age > 120)
            errors.Add("Age must be between 0 and 120.");

        if (errors.Any())
        {
            context.IsSuccessful = false;
            context.Errors.AddRange(errors);

            audit.WasSuccessful = false;
            audit.Errors.AddRange(errors);
        }
        else
        {
            audit.WasSuccessful = true;
        }

        audit.EndedAt = System.DateTime.UtcNow;
        context.StepAudits.Add(audit);

        return new PipelineContext<Person>
        {
            Data = context.Data,
            Metadata = context.Metadata,
            IsSuccessful = context.IsSuccessful,
            Errors = context.Errors,
            CreatedAt = context.CreatedAt,
            CorrelationId = context.CorrelationId,
            StepAudits = context.StepAudits
        };
    }

    public bool Validate(PipelineContext<Person> input)
    {
        // Ensure the input has a valid value
        return string.IsNullOrEmpty(input.Data.Name);
    }
}
