using Lesson01.API.DTO;

namespace Lesson01.API.Interfaces
{
    public interface IExamServiceAdapter
    {
        Task<Result<IEnumerable<ExamScoreModel>>> GetScores(string studentId, string examCode);
    }
}