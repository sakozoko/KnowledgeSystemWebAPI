namespace Application.ViewModels;

public class QuestionDto
{
    public string? Text { get; set; }
    public ICollection<AnswerDto>? Answers { get; set; }
    public decimal? Mark { get; set; }
}