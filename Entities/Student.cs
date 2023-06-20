using MicroServices.Shared.Extensions;

namespace Lesson01.API.Entities
{
    public class Student : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Nationality { get; set; }
    }
}