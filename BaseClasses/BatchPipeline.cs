namespace Pipeline;

public class BatchPipeline<TInput, TOutput>
{
    private readonly Func<PipelineContext<TInput>, PipelineContext<TOutput>> _pipelineFunc;

    public BatchPipeline(Func<PipelineContext<TInput>, PipelineContext<TOutput>> pipelineFunc)
    {
        _pipelineFunc = pipelineFunc;
    }

    public List<PipelineContext<TOutput>> Execute(IEnumerable<PipelineContext<TInput>> contexts)
    {
        var resultList = new List<PipelineContext<TOutput>>();

        foreach (var context in contexts)
        {
            var result = _pipelineFunc(context);
            resultList.Add(result);
        }

        return resultList;
    }
}