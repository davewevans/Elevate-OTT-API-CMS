using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.CreateCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.DeleteCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.UpdateCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoryForEdit;

namespace OttApiPlatform.WebAPI.Controllers;

//[BpCategoryize]
[AllowAnonymous]
[Route("api/categories")]
[ApiController]
public class CategoriesController : ApiController
{
    #region Public Methods

    [HttpGet("{id:guid}", Name = "CategoryById")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var httpRequest = Request;
        var response = await MediatR.Mediator.Send(new GetCategoryForEditQuery { Id = id });
        return TryGetResult(response);
    }

    //[AutoWrapIgnore]
    [HttpPost]
    public async Task<IActionResult> GetCategories([FromBody] GetCategoriesQuery request)
    {
        var httpRequest = Request;
        var response = await MediatR.Mediator.Send(request);

        return TryGetResult(response);
    }

    //[HttpPost("auto-complete")]
    //public async Task<IActionResult> GetCategoriesForAutoComplete([FromBody] GetCategoriesForAutoCompleteQuery request)
    //{
    //    var httpRequest = Request;
    //    var response = await MediatR.Mediator.Send(request);

    //    return TryGetResult(response);
    //}

    [HttpPost("add")]
    [RequestFormLimits(MultipartBodyLengthLimit = 20971520)] // 20MB
    public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryCommand request)
    {
        var response = await MediatR.Mediator.Send(request);
        return TryGetResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand request)
    {
        var response = await MediatR.Mediator.Send(request);
        return TryGetResult(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var response = await MediatR.Mediator.Send(new DeleteCategoryCommand { Id = id });
        return TryGetResult(response);
    }

    #endregion Public Methods
}
