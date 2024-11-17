using System.ComponentModel;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Commands.CreateCategory;

namespace OttApiPlatform.CMS.Pages.ContentManagement.Categories;

public partial class AddCategory : ComponentBase
{
    #region Private Properties
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] private IDialogService? DialogService { get; set; }
    [Inject] private ISnackbar? Snackbar { get; set; }
    [Inject] private IBreadcrumbService? BreadcrumbService { get; set; }
    [Inject] private ICategoriesClient? CategoriesClient { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }


    private string? _categoryImageSrc;

    //
    // TODO these values should come from config
    // TODO replace text with Resources
    // TODO server side validators????
    //
    private string _recommendedResolution = "700x700";
    private string _slugExampleName = "strength-training";
    private int _maxNameChars = 60;
    private int _maxSeoTitleChars = 60;
    private int _maxSeoDescriptionChars = 170;
    private int _maxSlugChars = 60;


    private string SlugPlaceholder => _slugExampleName;

    // TODO getters and setters ??????
    private StreamContent? _imageContent { get; set; }
    //private ServerSideValidator? _serverSideValidator { get; set; }
    // private EditContextServerSideValidator? _editContextServerSideValidator { get; set; }
    private CreateCategoryCommand _createCategoryCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods
    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService?.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Categories, "/content/categories"),
            new(Resource.Add_Category, "#", true)
        });

        _createCategoryCommand.PropertyChanged += NameChangedHandler;
    }
    #endregion Protected Methods

    #region Private Methods
    private void NameChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        _createCategoryCommand.Slug = _createCategoryCommand.Slug.FormatSlug();
        StateHasChanged();
    }
    private void GetBase64StringImageUrl(string imageSrc)
    {
        _categoryImageSrc = imageSrc;
        StateHasChanged();
    }

    private void ImageSelected(StreamContent content)
    {
        _imageContent = content;
        _createCategoryCommand.IsImageAdded = true;
        StateHasChanged();
    }

    private void ImageUnSelected()
    {
        _imageContent = null;
        _createCategoryCommand.IsImageAdded = false;
        StateHasChanged();
    }

    private bool HasUploadedImage()
    {
        return !string.IsNullOrWhiteSpace(_categoryImageSrc);
    }

    private void RemoveCategoryImage()
    {
        _imageContent = null;
        _categoryImageSrc = null;
        if (_createCategoryCommand?.ImageUrl is not null)
        {
            _createCategoryCommand.ImageUrl = null;
        }
        StateHasChanged();
    }

    private void UpdateRteValue(string value)
    {
        _createCategoryCommand.Description = value;
    }

    private async Task SubmitForm()
    {
        // TODO guard clauses

        Console.WriteLine("SubmitForm");

        var userFormData = new MultipartFormDataContent
        {
            { new StringContent(_createCategoryCommand.Title ?? string.Empty), "Title" },
            { new StringContent(_createCategoryCommand.Description ?? string.Empty), "Description" },
            { new StringContent(_createCategoryCommand.Position.ToString()), "Position" },
            { new StringContent(_createCategoryCommand.ImageUrl ?? string.Empty), "ImageUrl" },
            { new StringContent(_createCategoryCommand.SeoTitle ?? string.Empty), "SeoTitle" },
            { new StringContent(_createCategoryCommand.SeoDescription ?? string.Empty), "SeoDescription" },
            { new StringContent(_createCategoryCommand.Slug ?? string.Empty), "Slug" },
            { new StringContent(_createCategoryCommand.IsImageAdded.ToString()), "IsImageAdded" },
        };

        if (_imageContent != null)
            userFormData.Add(_imageContent, "ImageFile", _imageContent.Headers.GetValues("FileName").LastOrDefault());

        Console.WriteLine("pre update call");

        var responseWrapper = await CategoriesClient.CreateCategory(userFormData);

        Console.WriteLine("post update call");

        Console.WriteLine("httpResponse: " + responseWrapper);
        Console.WriteLine("httpResponse.Success: " + responseWrapper.IsSuccessStatusCode);


        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager?.NavigateTo("content/categories");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
    #endregion Private Methods
}
