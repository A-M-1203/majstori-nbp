namespace majstori_nbp_server.Services;

public interface IEmailService
{
    IEnumerable<string> GetAllEmails();
    Task<bool> CheckIfExistsAsync(string email);
    Task<bool> CreateEmailAsync(string email);
    Task<bool> UpdateEmailAsync(string newEmail, string oldEmail);
    Task<bool> DeleteEmailAsync(string email);
}