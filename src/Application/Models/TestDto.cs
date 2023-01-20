namespace Application.Models;

public class TestDto : BaseViewModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<QuestionDto>? Question { get; set; }
    public decimal? MaxMark { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public int? UserCreatorId { get; set; }
}