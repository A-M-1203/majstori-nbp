using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class EmailService : IEmailService
{
    private readonly ICacheService _cacheService;

    public EmailService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public IEnumerable<string> GetAllEmails(string role)
    {
        return _cacheService.GetAllSetData(role + "emails");
    }

    public async Task<string?> GetEmailAsync(string email)
    {
        return await _cacheService.GetDataAsync(email);
    }

    public async Task<bool> CheckIfExistsAsync(string role, string email)
    {
        return await _cacheService.SetDataExistsAsync(role + "emails", email);
    }

    public async Task<bool> CreateEmailAsync(string role, string email, string userId)
    {
        bool created = await _cacheService.CreateSetDataAsync(role + "emails", email);
        if (created is false)
        {
            return false;
        }

        return await _cacheService.CreateDataAsync(email, userId);
    }

    public async Task<bool> UpdateEmailAsync(string role, string newEmail, string oldEmail, string userId)
    {
        await DeleteEmailAsync(role, oldEmail);
        return await CreateEmailAsync(role, newEmail, userId);
    }

    public async Task<bool> DeleteEmailAsync(string role, string email)
    {
        return await _cacheService.DeleteSetDataAsync(role + "emails", email);
    }
}