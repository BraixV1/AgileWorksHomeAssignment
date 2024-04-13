namespace Domain;

public class ToDoTask : BaseEntity
{
    public string Description { get; set; } = default!;

    public DateTime CreatedAtDt { get; set; } = default!;

    public DateTime HasToBeDoneAtDt { get; set; } = default!;

    public DateTime? CompletedAtDt { get; set; } = default!;
}