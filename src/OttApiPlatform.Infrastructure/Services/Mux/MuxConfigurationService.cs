using OttApiPlatform.Application.Common.Contracts;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Infrastructure.Services.Mux;

public class MuxConfigurationService : IMuxConfigurationService
{
    private readonly ApplicationDbContext _context;
    private readonly ICryptoService _encryptionService;
    private readonly ITenantResolver _tenantResolver;

    public MuxConfigurationService(ApplicationDbContext context, ICryptoService encryptionService, ITenantResolver tenantResolver)
    {
        _context = context;
        _encryptionService = encryptionService;
        _tenantResolver = tenantResolver;
    }

    public async Task<MuxSettingsModel> GetMuxConfigForTenantAsync(Guid tenantId)
    {
        var config = await _context.MuxSettings.FirstOrDefaultAsync(c => c.TenantId == tenantId);
        if (config == null) throw new Exception("Mux configuration not found.");

        config.TokenId = _encryptionService.DecryptText(config.TokenId);
        config.TokenSecret = _encryptionService.DecryptText(config.TokenSecret);

        return config;
    }

    public async Task SaveMuxConfigAsync(Guid tenantId, string tokenId, string tokenSecret)
    {
        var encryptedTokenId = _encryptionService.EncryptText(tokenId);
        var encryptedTokenSecret = _encryptionService.EncryptText(tokenSecret);

        var config = await _context.MuxSettings.FirstOrDefaultAsync(c => c.TenantId == tenantId);
        if (config == null)
        {
            config = new MuxSettingsModel()
            {
                TokenId = encryptedTokenId,
                TokenSecret = encryptedTokenSecret
            };
            await _context.MuxSettings.AddAsync(config);
        }
        else
        {
            config.TokenId = encryptedTokenId;
            config.TokenSecret = encryptedTokenSecret;
        }

        await _context.SaveChangesAsync();
    }
}

