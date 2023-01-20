using Application.Models;
using Application.ViewModels;
using Domain.Entities;

namespace Application.Extension.Mapping;

public static class FromDtoToEntityMapper
{
    public static AnswerEntity ToAnswerEntity(this AnswerDto answerDto)
    {
        return new AnswerEntity
        {
            Id = answerDto.Id,
            Text = answerDto.Text,
            IsCorrect = answerDto.IsCorrect
        };
    }

    public static IEnumerable<AnswerEntity> ToAnswerEntities(this IEnumerable<AnswerDto> answerDtos)
    {
        return answerDtos.Select(ToAnswerEntity);
    }

    public static QuestionEntity ToQuestionEntity(this QuestionDto questionDto)
    {
        return new QuestionEntity
        {
            Id = questionDto.Id,
            Text = questionDto.Text,
            Answers = questionDto.Answers?.ToAnswerEntities().ToList(),
            Mark = questionDto.Mark
        };
    }

    public static IEnumerable<QuestionEntity> ToQuestionsEntities(this IEnumerable<QuestionDto> questionDtos)
    {
        return questionDtos.Select(ToQuestionEntity);
    }
}