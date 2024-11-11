using Microsoft.AspNetCore.Mvc;

namespace OttApiPlatform.WebAPI.Controllers.TenantSubscribers;

[Route("api/[controller]")]
[BpAuthorize]
public class TenantSubscribersController : ApiController
{

}
