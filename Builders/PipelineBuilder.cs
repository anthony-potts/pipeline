using Pipeline.Interfaces;

namespace Pipeline;

public class PipelineBuilder<TInput, TCurrent>
{
    private readonly Func<PipelineContext<TInput>, PipelineContext<TCurrent>> _currentPipeline;
    private readonly IPipelineContextRepository _repository;

    public PipelineBuilder(Func<PipelineContext<TInput>, PipelineContext<TCurrent>> currentPipeline,
        IPipelineContextRepository repository)
    {
        _currentPipeline = currentPipeline;
        _repository = repository;
    }

    // Method to add a step to the pipeline
    public PipelineBuilder<TInput, TNext> AddStep<TNext>(IPipelineStep<TCurrent, TNext> step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));

        Func<PipelineContext<TInput>, PipelineContext<TNext>> newPipeline = context =>
        {
            if (!context.IsSuccessful)
            {
                return new PipelineContext<TNext>
                {
                    Data = default(TNext),
                    Metadata = new Dictionary<string, object>(context.Metadata),
                    IsSuccessful = context.IsSuccessful,
                    Errors = new List<string>(context.Errors),
                    CreatedAt = context.CreatedAt,
                    CorrelationId = context.CorrelationId,
                    StepAudits = new List<PipelineStepAudit>(context.StepAudits)
                };
            }

            var currentContext = _currentPipeline(context);
            var outputContext = step.Process(currentContext);
            // Persist the context after processing the step
            _repository.SaveContextAsync(outputContext).Wait();
            return outputContext;
        };

        return new PipelineBuilder<TInput, TNext>(newPipeline, _repository);
    }

    // Method to build the final pipeline function
    public Func<PipelineContext<TInput>, PipelineContext<TCurrent>> Build()
    {
        return context => _currentPipeline(context);
    }
}