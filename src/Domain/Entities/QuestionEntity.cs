namespace Domain.Entities;

public class QuestionEntity : BaseEntity
{
    public string? Text { get; set; }
    public TestEntity? Test { get; set; }
    public decimal? Mark { get; set; }
    public ICollection<AnswerEntity>? Answers { get; set; }
}