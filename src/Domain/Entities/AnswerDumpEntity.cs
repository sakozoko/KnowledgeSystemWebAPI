namespace Domain.Entities;

public class AnswerDumpEntity : BaseEntity
{
    public PassedTestEntity? PassedTest { get; set; }
    public QuestionEntity? Question { get; set; }
    public AnswerEntity? RespondedAnswer { get; set; }
}