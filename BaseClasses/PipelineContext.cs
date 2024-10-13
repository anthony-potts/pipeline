namespace Pipeline;

public class PipelineContext<T>
{
    public T Data { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new ();
    public bool IsSuccessful { get; set; } = true;
    public List<string> Errors { get; set; } = new ();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public Guid CorrelationId { get; set; } = Guid.NewGuid();

    public List<PipelineStepAudit> StepAudits { get; set; } = new();
}
