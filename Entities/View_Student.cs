using MicroServices.Shared.Extensions;

namespace Lesson01.API.Entities
{
    public class View_Student : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string EmailAddress { get; set; }
        public string Nationality { get; set; }
    }
}