using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OttApiPlatform.Domain.Enums;

namespace OttApiPlatform.Application.Common.Contracts.WebAPI.Hub;

public interface IAssetHubNotificationService
{
    Task SendMessage(string user, string message);
    Task BroadcastMessage(string message);
    Task NotifyCreationStatus(Guid assetId, AssetCreationStatus status);
}
