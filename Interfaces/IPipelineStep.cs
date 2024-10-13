namespace Pipeline.Interfaces;

public interface IPipelineStep<TInput, TOutput>
{
    PipelineContext<TOutput> Process(PipelineContext<TInput> input);
}

