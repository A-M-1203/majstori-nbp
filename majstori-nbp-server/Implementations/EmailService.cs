using majstori_nbp_server.Services;

namespace majstori_nbp_server.Implementations;

public class EmailService : IEmailService
{
    private readonly ICacheService _cacheService;

    public EmailService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public IEnumerable<string> GetAllEmails()
    {
        return _cacheService.GetAllSetData("emails");
    }

    public async Task<bool> CheckIfExistsAsync(string email)
    {
        return await _cacheService.GetSetDataAsync("emails", email);
    }

    public async Task<bool> CreateEmailAsync(string email)
    {
        return await _cacheService.CreateSetDataAsync("emails", email);
    }

    public async Task<bool> UpdateEmailAsync(string newEmail, string oldEmail)
    {
        await DeleteEmailAsync(oldEmail);
        return await CreateEmailAsync(newEmail);
    }

    public async Task<bool> DeleteEmailAsync(string email)
    {
        return await _cacheService.DeleteSetDataAsync("emails", email);
    }
}