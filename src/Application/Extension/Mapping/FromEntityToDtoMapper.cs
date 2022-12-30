using Application.ViewModels;
using Domain.Entities;

namespace Application.Extension.Mapping;

public static class FromEntityToDtoMapper
{
    public static AnswerDto ToAnswerDto(this AnswerEntity answer)
    {
        return new AnswerDto
        {
            Id = answer.Id,
            Text = answer.Text,
            IsCorrect = answer.IsCorrect
        };
    }
    public static IEnumerable<AnswerDto> ToAnswerDtos(this IEnumerable<AnswerEntity> answers)
    {
        return answers.Select(x => x.ToAnswerDto());
    }

    public static QuestionDto ToQuestionDto(this QuestionEntity question)
    {
        return new QuestionDto()
        {
            Id = question.Id,
            Text = question.Text,
            Answers = question.Answers?.ToAnswerDtos(),
            Mark = question.Mark
        };
    }
    
    public static IEnumerable<QuestionDto> ToQuestionDtos(this IEnumerable<QuestionEntity> questions)
    {
        return questions.Select(x => x.ToQuestionDto());
    }
    
    public static TestDto ToTestDto(this TestEntity test)
    {
        return new TestDto()
        {
            Id = test.Id,
            Title = test.Title,
            Question = test.Questions?.ToQuestionDtos(),
            CreationDate = test.CreatedDate,
            LastUpdateDate = test.LastModifiedDate,
            Description = test.Description,
            MaxMark = test.MaxMark,
            UserCreatorId = test.UserCreator.Id
        };
    }
    public static IEnumerable<TestDto> ToTestDtos(this IEnumerable<TestEntity> tests)
    {
        return tests.Select(x => x.ToTestDto());
    }
    
    public static AnswerDumpDto ToAnswerDumpDto(this AnswerDumpEntity answer)
    {
        return new AnswerDumpDto
        {
            Id = answer.Id,
            AnswerId = answer.Answer?.Id,
            QuestionId = answer.Question?.Id,
        };
    }
    public static IEnumerable<AnswerDumpDto> ToAnswerDumpDtos(this IEnumerable<AnswerDumpEntity> answers)
    {
        return answers.Select(x => x.ToAnswerDumpDto());
    }

    public static PassedTestDto ToPassedTestDto(this PassedTestEntity passedTest)
    {
        return new PassedTestDto()
        {
            Id = passedTest.Id,
            Mark = passedTest.Mark,
            PassedDate = passedTest.PassedDate,
            StartedDate = passedTest.StartedDate,
            Answers = passedTest.Answers?.ToAnswerDumpDtos(),
            TestId = passedTest.Test?.Id,
            UserId = passedTest.User?.Id
        };
    }

    public static IEnumerable<PassedTestDto> ToPassedTestDtos(this IEnumerable<PassedTestEntity> passedTests)
    {
        return passedTests.Select(pt => pt.ToPassedTestDto());
    }
    //todo remove password
    public static UserDto ToUserDto(this UserEntity user)
    {
        return new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password,
            Email = user.Email,
            FirstName = user.FirstName,
            Surname = user.Surname,
            RoleId = user.Role?.Id,
            CreatedTests = user.CreatedTests?.ToTestDtos(),
            PassedTests = user.PassedTests?.ToPassedTestDtos(),
            CreatedDate = user.CreatedDate
        };
    }
    
    public static IEnumerable<UserDto> ToUserDtos(this IEnumerable<UserEntity> users)
    {
        return users.Select(x => x.ToUserDto());
    }
}