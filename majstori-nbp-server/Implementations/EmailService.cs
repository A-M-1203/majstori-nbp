using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class EmailService : IEmailService
{
    private readonly ICacheService _cacheService;

    public EmailService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<string>> GetAllEmailsAsync(string role)
    {
        var list = new List<string>();
        await foreach (var email in _cacheService.GetAllSetDataAsync(role + "emails"))
        {
            list.Add(email);
        }
        return list;
    }

    public async Task<string?> GetEmailAsync(string email)
    {
        return await _cacheService.GetStringAsync(email);
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

        return await _cacheService.CreateOrUpdateStringAsync(email, userId);
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