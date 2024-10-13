using Pipeline.Interfaces;

namespace Pipeline.Extensions;

public static class PipelineStepExtensions
{
    public static IPipelineStep<TInput, TOutput> WithOutput<TInput, TOutput>(
        this IPipelineStep<TInput, TOutput> step, 
        string outputFilePath)
    {
        return new StepWithOutput<TInput, TOutput>(step, outputFilePath);
    }
}
