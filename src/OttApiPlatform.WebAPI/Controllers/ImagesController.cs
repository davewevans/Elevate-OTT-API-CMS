using Microsoft.AspNetCore.Mvc;

namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ImagesController : Controller
{
	#region Public Methods

	public IActionResult Index()
	{
		return View();
	} 

	#endregion
}
