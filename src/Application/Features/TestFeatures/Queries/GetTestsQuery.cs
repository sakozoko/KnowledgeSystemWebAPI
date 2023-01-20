using Application.Extension.Mapping;
using Application.Interfaces.Repositories;
using Application.Models;
using Application.ViewModels;
using MediatR;

namespace Application.Features.TestFeatures.Queries;

public class GetTestsQuery : IRequest<IEnumerable<TestDto>>
{
    public class GetTestsQueryHandler : IRequestHandler<GetTestsQuery, IEnumerable<TestDto>>
    {
        private readonly ITestRepository _testRepository;

        public GetTestsQueryHandler(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<IEnumerable<TestDto>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {
            var tests = await _testRepository.GetAllWithDetailsAsync(cancellationToken);
            return tests.ToTestDtos();
        }
    }
}