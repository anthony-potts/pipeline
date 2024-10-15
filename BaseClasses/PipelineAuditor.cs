using Microsoft.Extensions.Logging;

namespace Pipeline;

public class PipelineAuditor
{
    private readonly ILogger<PipelineAuditor> _logger;

    // Inject a logger (or use a console output if you prefer)
    public PipelineAuditor(ILogger<PipelineAuditor> logger)
    {
        _logger = logger;
    }

    // General method to output audit steps from the pipeline context
    public void OutputAudit(PipelineContext context)
    {
        if (context.StepAudits.Count == 0)
        {
            _logger.LogInformation("No steps available in the pipeline to audit.");
            return;
        }

        _logger.LogInformation("===== Pipeline Audit Log =====");
        foreach (var step in context.StepAudits)
        {
            _logger.LogInformation(
                $"Step: {step.StepName} | Success: {step.WasSuccessful} | From: {step.StartedAt} To: {step.EndedAt}");
            if (step.Errors.Count > 0)
            {
                _logger.LogInformation("====STEP ERRORS");
                foreach (var error in step.Errors)
                {
                    _logger.LogInformation(error);
                }

            }

        }
        _logger.LogInformation("===== End of Pipeline Audit Log =====");
    }
}