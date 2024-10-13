namespace Pipeline;

public interface IPipelineContextRepository
{
    Task SaveContextAsync<T>(PipelineContext<T> context);
}