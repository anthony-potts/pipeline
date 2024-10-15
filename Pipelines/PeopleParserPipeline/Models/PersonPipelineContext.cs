namespace Pipeline.Pipelines.Models;

public class PersonPipelineContext: PipelineContext
{
    public List<Person> People = new();
    public string OutputList = string.Empty;
}