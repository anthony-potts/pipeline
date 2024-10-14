using Pipeline;
using Pipeline.Interfaces;

public class StepWithOutput<TInput, TOutput> : IPipelineStep<TInput, TOutput>
{
    private readonly IPipelineStep<TInput, TOutput> _innerStep;
    private readonly string _outputFilePath;

    public StepWithOutput(IPipelineStep<TInput, TOutput> innerStep, string outputFilePath)
    {
        _innerStep = innerStep ?? throw new ArgumentNullException(nameof(innerStep));
        _outputFilePath = outputFilePath ?? throw new ArgumentNullException(nameof(outputFilePath));
    }

    public PipelineContext<TOutput> Process(PipelineContext<TInput> input)
    {
        // Execute the wrapped step
        var outputContext = _innerStep.Process(input);

        // Save output to file if the step was successful
        if (outputContext.IsSuccessful)
        {
            File.WriteAllText(_outputFilePath, outputContext.Data?.ToString() ?? string.Empty);
        }

        return outputContext;
    }

    public bool Validate(PipelineContext<TInput> input)
    {
        return _innerStep.Validate(input);
    }
}