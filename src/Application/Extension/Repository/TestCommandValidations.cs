using Application.Interfaces.Repositories;

namespace Application.Extension.Repository;

public static class TestCommandValidations
{
    public static bool NewMaxMarkEqualsSumOfQuestionMarks(this ITestRepository testRepository, int testId, decimal newMaxMark)
    {
        var test = testRepository.GetByIdAsync(testId).Result;
        var sumOfQuestionMarks = test?.Questions?.Sum(q => q.Mark);
        return newMaxMark == sumOfQuestionMarks;
    }
    
    public static bool IsTestExist(this ITestRepository testRepository, int testId)
    {
        return testRepository.GetByIdAsync(testId).Result is not null;
    }
}