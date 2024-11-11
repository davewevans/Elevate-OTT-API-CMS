using OttApiPlatform.Domain.Enums.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Common.Contracts.Hub;
public interface IVideoHubNotificationService
{
    Task NotifyCreationStatus(Guid videoId, AssetCreationStatus status);
}
