using System.Text.Json.Serialization;

namespace Domain.Entities;

public class PassedTestEntity : BaseEntity
{
    public TestEntity? Test { get; set; }
    public decimal? Mark { get; set; }
    public DateTime? StartedDate { get; set; }
    public DateTime? PassedDate { get; set; }
    public UserEntity? User { get; set; }
    public ICollection<AnswerDumpEntity>? Answers { get; set; }
}