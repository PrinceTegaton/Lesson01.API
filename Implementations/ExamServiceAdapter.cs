using Lesson01.API.DTO;
using Lesson01.API.Interfaces;

namespace Lesson01.API.Implementations
{
    public class ExamServiceAdapter : IExamServiceAdapter
    {
        public ExamServiceAdapter()
        {
            // initialize httpclient
        }

        public async Task<Result<IEnumerable<ExamScoreModel>>> GetScores(string studentId, string examCode)
        {
            return await Task.FromResult(Result<IEnumerable<ExamScoreModel>>.Success(new List<ExamScoreModel>(), "Success"));
        }
    }
}
