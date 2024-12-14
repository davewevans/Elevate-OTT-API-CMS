using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Infrastructure.Services.Mux;
public class MuxConfigurationFactory : IMuxConfigurationFactory
{
    private readonly MuxOptions _managedMuxSettings;
    private readonly IApplicationDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public MuxConfigurationFactory(IConfigReaderService configReaderService, IApplicationDbContext dbContext, IMemoryCache cache)
    {
        _managedMuxSettings = configReaderService.GetMuxOptions();
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<MuxSettingsModel> GetMuxSettingsAsync(Guid tenantId, bool useManagedSettings)
    {
        if (useManagedSettings)
        {
            // Use managed Mux settings
            return new MuxSettingsModel
            {
                TokenId = _managedMuxSettings.TokenId,
                TokenSecret = _managedMuxSettings.TokenSecret,
                SigningSecret = _managedMuxSettings.SigningSecret,
            };
        }

        // Check cache for user-specific settings
        if (_cache.TryGetValue($"MuxSettings_{tenantId}", out MuxSettingsModel cachedSettings))
        {
            return cachedSettings;
        }

        // Query the database for user-specific settings
        var muxConfig = await _dbContext.MuxSettings
            .FirstOrDefaultAsync(m => m.TenantId == tenantId);

        if (muxConfig == null)
        {
            throw new Exception("Mux configuration not found for the tenant.");
        }

        var userMuxSettings = new MuxSettingsModel
        {
            TokenId = muxConfig.TokenId,
            TokenSecret = muxConfig.TokenSecret,
            SigningSecret = muxConfig.SigningSecret,
            Username = muxConfig.Username,
            Password = muxConfig.Password,
            CorsOrigin = muxConfig.CorsOrigin,
            RtmpUrl = muxConfig.RtmpUrl,
            RtmpsUrl = muxConfig.RtmpsUrl
        };

        // Cache the result
        _cache.Set($"MuxSettings_{tenantId}", userMuxSettings, TimeSpan.FromHours(1));

        return userMuxSettings;
    }
}
