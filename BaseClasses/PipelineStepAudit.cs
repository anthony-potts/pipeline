namespace Pipeline;

public class PipelineStepAudit
{
    public string StepName { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool WasSuccessful { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}