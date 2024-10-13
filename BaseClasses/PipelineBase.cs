namespace Pipeline;



public class PipelineBase<TInput, TOutput>
{
    private readonly Func<PipelineContext<TInput>, PipelineContext<TOutput>> _pipelineFunc;

    internal PipelineBase(Func<PipelineContext<TInput>, PipelineContext<TOutput>> pipelineFunc)
    {
        _pipelineFunc = pipelineFunc;
    }

    public PipelineContext<TOutput> Execute(PipelineContext<TInput> context)
    {
        return _pipelineFunc(context);
    }

    public static PipelineBuilder<TInput, TInput> Create(IPipelineContextRepository repository)
    {
        return new PipelineBuilder<TInput, TInput>(_ => new PipelineContext<TInput>(), repository);
    }
}
