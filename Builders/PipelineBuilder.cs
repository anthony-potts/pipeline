using Pipeline.Interfaces;

namespace Pipeline;

public class PipelineBuilder<TContext> where TContext : PipelineContext
{
    private readonly List<IPipelineStep<TContext>> _steps = new();

    private readonly IPipelineContextRepository _repository;

    public PipelineBuilder(IPipelineContextRepository repository)
    {
        _repository = repository;
    }

    // Method to add a step to the pipeline
    public PipelineBuilder<TContext> AddStep(IPipelineStep<TContext> step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        
        _steps.Add(step);
        return this;
    }

    // Method to build the final pipeline function
    public Func<TContext, TContext> Build<T>()
    {
        return context =>
        {
            foreach (var step in _steps)
            {
                if (!context.IsSuccessful)
                {
                    break;
                }

                var outputContext = step.Process(context);
                _repository.SaveContextAsync(outputContext).Wait();

                context = outputContext;
            }

            return context; //recurse
        };
    }
}