namespace Lesson01.API.DTO
{
    public class ExamScoreModel
    {
        public string CourseCode { get; set; }
        public double Score { get; set; }
        public string Grade { get; set; }

        public ExamScoreModel()
        {
            
        }

        public ExamScoreModel(string courseCode, double score, string grade)
        {
            CourseCode = courseCode;
            Score = score;
            Grade = grade;
        }
    }
}
