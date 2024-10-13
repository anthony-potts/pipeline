namespace Pipeline.Interfaces;

public interface IPipelineStepAsync<TInput, TOutput>
{
    Task<PipelineContext<TOutput>> ProcessAsync(PipelineContext<TInput> input);
}