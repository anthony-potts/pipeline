using Pipeline.Interfaces;
using Pipeline.Pipelines.OrderParserPipeline.Models;

namespace Pipeline.Pipelines.OrderParserPipeline.Steps;

public class ValidateOrderStep : IPipelineStep<Order, Order>
{
    public PipelineContext<Order> Process(PipelineContext<Order> context)
    {
        var audit = new PipelineStepAudit
        {
            StepName = nameof(ValidateOrderStep),
            StartedAt = DateTime.UtcNow
        };

        var errors = new List<string>();

        if (string.IsNullOrEmpty(context.Data.OrderID))
            errors.Add("OrderID cannot be empty.");
        if (string.IsNullOrEmpty(context.Data.Product))
            errors.Add("Product cannot be empty.");
        if (context.Data.Quantity <= 0)
            errors.Add("Quantity must be greater than zero.");

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

        audit.EndedAt = DateTime.UtcNow;
        context.StepAudits.Add(audit);

        return context; // Data remains the same
    }
}