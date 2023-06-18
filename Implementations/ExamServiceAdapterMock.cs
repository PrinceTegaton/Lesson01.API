using Lesson01.API.DTO;
using Lesson01.API.Interfaces;

namespace Lesson01.API.Implementations
{
    public class ExamServiceAdapterMock : IExamServiceAdapter
    {
        public async Task<Result<IEnumerable<ExamScoreModel>>> GetScores(string studentId, string examCode)
        {
            return await Task.FromResult(Result<IEnumerable<ExamScoreModel>>.Success(new List<ExamScoreModel>
            {
                new("COM411", 91, "A"),
                new("COM412", 78, "B"),
                new("COM413", 55, "C"),
                new("COM414", 89, "B+"),
            }, "Success"));
        }
    }
}
