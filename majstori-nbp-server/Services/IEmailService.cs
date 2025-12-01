namespace majstori_nbp_server.Services;

public interface IEmailService
{
    IEnumerable<string> GetAllEmails(string role);
    Task<string?> GetEmailAsync(string email);
    Task<bool> CheckIfExistsAsync(string role, string email);
    Task<bool> CreateEmailAsync(string role, string email, string userId);
    Task<bool> UpdateEmailAsync(string role, string newEmail, string oldEmail, string userId);
    Task<bool> DeleteEmailAsync(string role, string email);
}