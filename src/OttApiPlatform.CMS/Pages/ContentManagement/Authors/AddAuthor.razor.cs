using System.ComponentModel;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Commands.CreateAuthor;

namespace OttApiPlatform.CMS.Pages.ContentManagement.Authors;

public partial class AddAuthor : ComponentBase
{
    #region Private Properties
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAuthorsClient AuthorsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }


    private string? _authorImageSrc;

    //
    // TODO these values should come from config
    //
    private string _recommendedResolution = "700x700";
    private string _slugExampleName = "carey-bryers";
    private int _maxNameChars = 60;
    private int _maxSeoTitleChars = 60;
    private int _maxSeoDescriptionChars = 170;
    private int _maxSlugChars = 60;


    private string SlugPlaceholder => _slugExampleName;

    // TODO getters and setters ??????
    private StreamContent? _imageContent { get; set; }
    private CreateAuthorCommand _createAuthorCommand { get; } = new ();

    #endregion Private Properties

    #region Protected Methods
    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService?.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Authors, "/content/authors"),
            new(Resource.Add_Author, "#", true)
        });

        _createAuthorCommand.PropertyChanged += NameChangedHandler;
    }
    #endregion Protected Methods

    #region Private Methods
    private void NameChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        _createAuthorCommand.Slug = _createAuthorCommand.Name.FormatSlug();
        StateHasChanged();
    }
    private void GetBase64StringImageUrl(string imageSrc)
    {
        _authorImageSrc = imageSrc;
        StateHasChanged();
    }

    private void ImageSelected(StreamContent content)
    {
        _imageContent = content;
        _createAuthorCommand.IsImageAdded = true;
        StateHasChanged();
    }

    private void ImageUnSelected()
    {
        _imageContent = null;
        _createAuthorCommand.IsImageAdded = false;
        StateHasChanged();
    }

    private bool HasUploadedImage()
    {
        return !string.IsNullOrWhiteSpace(_authorImageSrc);
    }

    private void RemoveAuthorImage()
    {
        _imageContent = null;
        _authorImageSrc = null;
        if (_createAuthorCommand?.ImageUrl is not null)
        {
            _createAuthorCommand.ImageUrl = null;
        }
        StateHasChanged();
    }

    private void UpdateRteValue(string value)
    {
        _createAuthorCommand.Bio = value;
    }

    private async Task SubmitForm()
    {
        // TODO guard clauses

        Console.WriteLine("SubmitForm");

        var userFormData = new MultipartFormDataContent
        {
            { new StringContent(_createAuthorCommand.Name ?? string.Empty), "Title" },
            { new StringContent(_createAuthorCommand.Bio ?? string.Empty), "Bio" },
            { new StringContent(_createAuthorCommand.ImageUrl ?? string.Empty), "ImageUrl" },
            { new StringContent(_createAuthorCommand.SeoTitle ?? string.Empty), "SeoTitle" },
            { new StringContent(_createAuthorCommand.SeoDescription ?? string.Empty), "SeoDescription" },
            { new StringContent(_createAuthorCommand.Slug ?? string.Empty), "Slug" },
            { new StringContent(_createAuthorCommand.IsImageAdded.ToString()), "IsImageAdded" },
        };

        if (_imageContent != null)
            userFormData.Add(_imageContent, "ImageFile", _imageContent.Headers.GetValues("FileName").LastOrDefault());

        Console.WriteLine("pre update call");

        var responseWrapper = await AuthorsClient.CreateAuthor(userFormData);

        Console.WriteLine("post update call");

        Console.WriteLine("httpResponse: " + responseWrapper);
        Console.WriteLine("httpResponse.Success: " + responseWrapper.IsSuccessStatusCode);


        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("content/authors");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
    #endregion Private Methods
}
