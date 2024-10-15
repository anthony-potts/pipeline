namespace Pipeline;

public interface IPipelineContextRepository
{
    Task SaveContextAsync(PipelineContext context);
}