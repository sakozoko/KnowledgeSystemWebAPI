namespace Application.Models;

public class QuestionDto : BaseViewModel
{
    public string? Text { get; set; }
    public IEnumerable<AnswerDto>? Answers { get; set; }
    public decimal? Mark { get; set; }
}