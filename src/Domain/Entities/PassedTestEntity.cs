namespace Domain.Entities;

public class PassedTestEntity : BaseEntity
{
    public TestEntity? Test { get; set; }
    public double? Mark { get; set; }
    public DateTime? PassedDate { get; set; }
    public UserEntity? User { get; set; }
    public ICollection<AnswerDumpEntity>? Answers { get; set; }
}
