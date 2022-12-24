namespace Domain.Entities;

public class TestEntity : BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double? MaxMark { get; set; }
    public UserEntity? UserCreator { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public ICollection<QuestionEntity>? Questions { get; set; }
}