using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OttApiPlatform.Domain.Enums;

namespace OttApiPlatform.Application.Common.Contracts.Hub;
public interface IVideoHubNotificationService
{
    Task NotifyCreationStatus(Guid videoId, AssetCreationStatus status);
}
