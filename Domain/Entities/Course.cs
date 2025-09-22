namespace Domain.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; }
    public int Credits { get; set; }
}