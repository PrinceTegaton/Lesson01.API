namespace Lesson01.API.Interfaces
{
    public interface IEmailAdapter
    {
        Task<Result<string>> SendEmail(string emailAddress, string subject, string body);
        Task<Result<string>> SendEmail(string emailAddress, string subject, EmailTemplate template, IDictionary<string, string> parameters);
    }
}
