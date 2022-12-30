using Application.Interfaces.Repositories;

namespace Application.Extension.Repository;

public static class TestCommandValidations
{
    public static bool NewMaxMarkEqualsSumOfQuestionMarks(this ITestRepository testRepository, int testId,
        decimal newMaxMark)
    {
        var test = testRepository.GetByIdAsync(testId).Result;
        var sumOfQuestionMarks = test?.Questions?.Sum(q => q.Mark);
        return newMaxMark == sumOfQuestionMarks;
    }
}