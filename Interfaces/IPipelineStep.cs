namespace Pipeline.Interfaces;

public interface IPipelineStep<TContext> where TContext : PipelineContext
{
    TContext Process(TContext context);
}

