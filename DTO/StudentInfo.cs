namespace Lesson01.API.DTO
{
    public class StudentInfo
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string EmailAddress { get; set; }
        public string Nationality { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}