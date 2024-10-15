using Pipeline.Interfaces;
using Pipeline.Pipelines.Models;

namespace Pipeline.Pipelines.Steps;

using System.Collections.Generic;
using System.Linq;

public class ValidateStep : IPipelineStep<PersonPipelineContext>
{
    public PersonPipelineContext Process(PersonPipelineContext context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(ValidateStep),
            StartedAt = System.DateTime.UtcNow
        };

        var inputPeople = context.People;
        var outputPeople = new List<Person>();

        var errors = new List<string>();

        foreach (var person in inputPeople)
        {
            var personErrors = new List<string>();
            if (string.IsNullOrEmpty(person.Name))
                personErrors.Add("Name cannot be empty.");
            if (person.Age < 0 || person.Age > 120)
                personErrors.Add("Age must be between 0 and 120.");

            if (personErrors.Any())
            {
                audit.WasSuccessful = false;
                audit.Errors.AddRange(personErrors);
            }
            else
            {
                audit.WasSuccessful = true;
                outputPeople.Add(person);
            }
        }
        
        audit.EndedAt = System.DateTime.UtcNow;
        context.StepAudits.Add(audit);
        
        

        return new PersonPipelineContext()
        {
            People = outputPeople,
            Metadata = context.Metadata,
            IsSuccessful = context.IsSuccessful,
            Errors = context.Errors,
            CreatedAt = context.CreatedAt,
            CorrelationId = context.CorrelationId,
            StepAudits = context.StepAudits
        };
    }
}
