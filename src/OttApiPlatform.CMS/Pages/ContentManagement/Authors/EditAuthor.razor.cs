using System.ComponentModel;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Commands.UpdateAuthor;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;

namespace OttApiPlatform.CMS.Pages.ContentManagement.Authors;

public partial class EditAuthor : ComponentBase
{
    #region Public Properties

    [Parameter] public Guid AuthorId { get; set; }

    #endregion Public Properties

    #region Private Properties
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] private IDialogService? DialogService { get; set; }
    [Inject] private ISnackbar? Snackbar { get; set; }
    [Inject] private IBreadcrumbService? BreadcrumbService { get; set; }
    [Inject] private IAuthorsClient? AuthorsClient { get; set; }

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
    private AuthorForEdit _authorForEditVm { get; set; } = new();
    private UpdateAuthorCommand? _updateAuthorCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods
    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService?.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Authors, "/content/authors"),
            new(Resource.Edit_Author, "#", true)
        });

        var responseWrapper = await AuthorsClient.GetAuthor(AuthorId);
        //responseWrapper.Payload.PropertyChanged += NameChangedHandler;

        if (responseWrapper.IsSuccessStatusCode)
        {
            _authorForEditVm = responseWrapper.Payload;
            _authorForEditVm.PropertyChanged += NameChangedHandler;

            if (!string.IsNullOrWhiteSpace(_authorForEditVm?.ImageUrl))
            {
                _authorImageSrc = _authorForEditVm.ImageUrl;
            }
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
    #endregion Protected Methods

    #region Private Methods
    private void NameChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        _authorForEditVm.Slug = _authorForEditVm.Name.FormatSlug();
        StateHasChanged();
    }

    private void GetBase64StringImageUrl(string imageSrc)
    {
        _authorImageSrc = imageSrc;
        Console.WriteLine(_authorImageSrc);
        StateHasChanged();
    }

    private void ImageSelected(StreamContent content)
    {
        _imageContent = content;
        _authorForEditVm.IsImageAdded = true;
        StateHasChanged();
    }

    private void ImageUnSelected()
    {
        _imageContent = null;
        _authorForEditVm.IsImageAdded = false;
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
        if (_authorForEditVm?.ImageUrl is not null)
        {
            _authorForEditVm.ImageUrl = null;
        }
        StateHasChanged();
    }
    private void UpdateRteValue(string value)
    {
        _authorForEditVm.Bio = value;
    }

    private async Task SubmitForm()
    {
        // TODO guard clauses

        Console.WriteLine("SubmitForm");

        _updateAuthorCommand = new UpdateAuthorCommand
        {
            Id = _authorForEditVm.Id,
            Name = _authorForEditVm.Name,
            Bio = _authorForEditVm.Bio,
            ImageUrl = _authorForEditVm.ImageUrl,
            SeoTitle = _authorForEditVm.SeoTitle,
            SeoDescription = _authorForEditVm.SeoDescription,
            Slug = _authorForEditVm.Slug.FormatSlug(),
            IsImageAdded = _authorForEditVm.IsImageAdded
        };


        var userFormData = new MultipartFormDataContent
            {
                { new StringContent(_updateAuthorCommand.Id.ToString() ?? string.Empty), "id" },
                { new StringContent(_updateAuthorCommand.Name ?? string.Empty), "Name" },
                { new StringContent(_updateAuthorCommand.Bio ?? string.Empty), "Bio" },
                { new StringContent(_updateAuthorCommand.ImageUrl ?? string.Empty), "ImageUrl" },
                { new StringContent(_updateAuthorCommand.SeoTitle ?? string.Empty), "SeoTitle" },
                { new StringContent(_updateAuthorCommand.SeoDescription ?? string.Empty), "SeoDescription" },
                { new StringContent(_updateAuthorCommand.Slug ?? string.Empty), "Slug" },
                { new StringContent(_updateAuthorCommand.IsImageAdded.ToString()), "IsImageAdded" },
            };

        if (_imageContent != null)
            userFormData.Add(_imageContent, "ImageFile", _imageContent.Headers.GetValues("FileName").LastOrDefault());

        Console.WriteLine("pre update call");

        var responseWrapper = await AuthorsClient.UpdateAuthor(userFormData);

        Console.WriteLine("post update call");

        Console.WriteLine("httpResponse: " + responseWrapper);
        Console.WriteLine("httpResponse.Success: " + responseWrapper.IsSuccessStatusCode);

        //if (httpResponse.Success)
        //{
        //    var successResult = httpResponse.Response as SuccessResult<string>;
        //    Snackbar?.Add(successResult?.Result, Severity.Success);
        //    NavigationManager?.NavigateTo("content/authors");
        //}
        //else
        //{
        //    var exceptionResult = httpResponse.Response as ExceptionResult;
        //    _editContextServerSideValidator?.Validate(exceptionResult);
        //    _serverSideValidator?.Validate(exceptionResult);
        //}

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
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
