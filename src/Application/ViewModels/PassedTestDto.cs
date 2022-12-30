namespace Application.ViewModels;

public class PassedTestDto : BaseViewModel
{
    public int? TestId { get; set; }
    public decimal? Mark { get; set; }
    public DateTime? StartedDate { get; set; }
    public DateTime? PassedDate { get; set; }
    public int? UserId { get; set; }
    public IEnumerable<AnswerDumpDto>? Answers { get; set; }
}