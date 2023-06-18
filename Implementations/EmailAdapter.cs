using Lesson01.API.Interfaces;

namespace Lesson01.API.Implementations
{
    public class EmailAdapter : IEmailAdapter
    {
        public async Task<Result<string>> SendEmail(string emailAddress, string subject, string body)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return Result<string>.Success(Guid.NewGuid().ToString());
        }

        public async Task<Result<string>> SendEmail(string emailAddress, string subject, EmailTemplate template, IDictionary<string, string> parameters)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return Result<string>.Success(Guid.NewGuid().ToString());
        }
    }
}
