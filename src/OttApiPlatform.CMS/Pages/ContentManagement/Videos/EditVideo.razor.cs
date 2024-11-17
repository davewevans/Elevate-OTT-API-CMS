using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorsForAutoComplete;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Commands.UpdateVideo;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideoForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;

namespace OttApiPlatform.CMS.Pages.ContentManagement.Videos;


// TODO Title text field is slow to update field while typing

public partial class EditVideo : ComponentBase
{
    #region Public Properties

    [Parameter] public Guid VideoId { get; set; }

    #endregion Public Properties

    #region Private Properties
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] private IDialogService? DialogService { get; set; }
    [Inject] private ISnackbar? Snackbar { get; set; }
    [Inject] private IBreadcrumbService? BreadcrumbService { get; set; }
    [Inject] private IVideosClient? VideosClient { get; set; }    
    [Inject] private IAuthorsClient? AuthorsClient { get; set; }
    [Inject] private ICategoriesClient? CategoriesClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }

    private string? _playerImageSrc;
    private string? _catalogImageSrc;
    private string? _featuredCatalogImageSrc;
    private string? _animatedGifSrc;

    private StreamContent? _playerImageContent;
    private StreamContent? _catalogImageContent;
    private StreamContent? _featuredCatalogImageContent;
    private StreamContent? _animatedGifContent;


    //
    // TODO these values should come from config
    //
    private EditContext _editContext;
    private string _recommendedResolution = "700x700";
    private string _slugExampleName = "carey-bryers";
    private int _maxNameChars = 60;
    private int _maxSeoTitleChars = 60;
    private int _maxSeoDescriptionChars = 170;
    private int _maxShortDescriptionChars = 140;
    private int _maxSlugChars = 60;

    #region Auto Complete Properties
    private AuthorItemForAutoComplete _selectedAuthor = new();
    private AuthorsForAutoCompleteResponse _authorsForAutoResponse = new ();

    private CategoryItemForAutoComplete _selectedCategory = new();
    private List<CategoryItemForAutoComplete> _selectedCategories;
    private CategoriesForAutoCompleteResponse _categoriesForAutoResponse = new();

    private VideoItemForAutoComplete _selectedTrailerVideo = new();
    private VideoItemForAutoComplete _selectedFeaturedCategoryVideo = new();
    private VideosForAutoCompleteResponse _videosForAutoCompleteResponse = new ();
    #endregion Auto Complete Properties

    private string SlugPlaceholder => _slugExampleName;

    //
    // TODO Check if this is needed
    // carryover from old project
    //

    //private ServerSideValidator? _serverSideValidator { get; set; }
    //private EditContextServerSideValidator? _editContextServerSideValidator { get; set; }
    private VideoForEdit _videoForEditVm { get; set; } = new();
    private UpdateVideoCommand? _updateVideoCommand { get; set; }



    // TODO move to class
    //
    private Variant _contentAccessFreeVariant = Variant.Filled;
    private Variant _contentAccessPremiumVariant = Variant.Outlined;
    private Color _contentAccessFreeIconColor = Color.Tertiary;
    private Color _contentAccessPremiumIconColor = Color.Secondary;

    private Variant _publicationStatusUnpublishedVariant = Variant.Filled;
    private Variant _publicationStatusPublishedVariant = Variant.Outlined;
    private Variant _publicationStatusScheduledVariant = Variant.Outlined;

    private Color _publicationStatusUnpublishedIconColor = Color.Tertiary;
    private Color _publicationStatusPublishedIconColor = Color.Secondary;
    private Color _publicationStatusScheduledIconColor = Color.Secondary;


    // private string base64String = "data:image/jpeg;base64,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";

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService?.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Videos, "/content/videos"),
            new(Resource.Edit_Video, "#", true)
        });

        var responseWrapper = await VideosClient.GetVideo(VideoId);

        if (responseWrapper.IsSuccessStatusCode)
        {
            _videoForEditVm = responseWrapper.Payload;

            SetImageSourcesIfExists();

            if (_videoForEditVm?.Author is not null)
            {
                _selectedAuthor = new AuthorItemForAutoComplete
                {
                    Id = _videoForEditVm.Author.Id,
                    Name = _videoForEditVm.Author.Name ?? string.Empty,
                    ImageUrl = _videoForEditVm.Author.ImageUrl ?? string.Empty
                };
            }
           
            if (_videoForEditVm.CategoryIds is not null)
            {
                var categoriesResponseWrapper = await CategoriesClient.GetCategoriesForAutoComplete(
                new GetCategoriesForAutoCompleteQuery
                {
                    PageNumber = 50,
                    SearchText = string.Empty
                });

                Console.WriteLine("categoriesResponseWrapper.Success" + categoriesResponseWrapper.IsSuccessStatusCode);

                if (categoriesResponseWrapper.IsSuccessStatusCode)
                {
                    _selectedCategories = new List<CategoryItemForAutoComplete>();
                    var categoriesSuccessResult = categoriesResponseWrapper.Payload;
                    if (categoriesSuccessResult != null)
                    {
                        foreach (var categoryId in _videoForEditVm.CategoryIds)
                        {
                            Console.WriteLine("categoryId: " + categoryId);

                            var categoryForAutoComplete = categoriesSuccessResult?.Categories?.Items.SingleOrDefault(c => c.Id.Equals(categoryId));
                            if (categoryForAutoComplete is not null)
                            {
                                _selectedCategories.Add(categoryForAutoComplete);

                            }
                        }
                    }                    
                }
            }

            if (_videoForEditVm?.TrailerVideo is not null)
            {
                _selectedTrailerVideo = _videoForEditVm?.TrailerVideo;
            }
            if (_videoForEditVm?.FeaturedCategoryVideo is not null)
            {
                _selectedFeaturedCategoryVideo = _videoForEditVm?.FeaturedCategoryVideo;
            }

            //_videoForEditVm.PropertyChanged += NameChangedHandler;

            if (_videoForEditVm != null)
            {
                SetContentAccess((int)_videoForEditVm.ContentAccess);
                SetPublicationStatus((int)_videoForEditVm.PublicationStatus);
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
    //private void NameChangedHandler(object? sender, PropertyChangedEventArgs e)
    //{
    //    _videoForEditVm.Slug = _videoForEditVm.Title.FormatSlug();
    //    StateHasChanged();
    //}

    #region Image Handler Methods
    private void SetImageSourcesIfExists()
    {
        if (!string.IsNullOrWhiteSpace(_videoForEditVm?.PlayerImageUrl))
        {
            _playerImageSrc = _videoForEditVm?.PlayerImageUrl;
        }

        if (!string.IsNullOrWhiteSpace(_videoForEditVm?.CatalogImageUrl))
        {
            _catalogImageSrc = _videoForEditVm?.CatalogImageUrl;
        }

        if (!string.IsNullOrWhiteSpace(_videoForEditVm?.FeaturedCatalogImageUrl))
        {
            _featuredCatalogImageSrc = _videoForEditVm?.FeaturedCatalogImageUrl;
        }

        if (!string.IsNullOrWhiteSpace(_videoForEditVm?.AnimatedGifUrl))
        {
            _animatedGifSrc = _videoForEditVm?.AnimatedGifUrl;
        }
    }

    private void GetBase64StringPlayerImageUrl(string base64String)
    {
        _playerImageSrc = base64String;
        StateHasChanged();
    }

    private void GetBase64StringCatalogImageUrl(string base64String)
    {
        _catalogImageSrc = base64String;
        StateHasChanged();
    }

    private void GetBase64StringFeaturedCatalogImageUrl(string base64String)
    {
        _featuredCatalogImageSrc = base64String;
        StateHasChanged();
    }

    private void GetBase64StringAnimatedGifUrl(string base64String)
    {
        _animatedGifSrc = base64String;
        StateHasChanged();
    }

    // TODO
    // more testing

    private void PlayerImageSelected(StreamContent content)
    {
        Console.WriteLine("PlayerImageSelected");
        _playerImageContent = content;
        _videoForEditVm.PlayerImageState = ImageState.Added;
        StateHasChanged();
    }

    private void PlayerImageUnSelected()
    {
        Console.WriteLine("PlayerImageUnSelected");

        _playerImageContent = null;
        _videoForEditVm.PlayerImageState = ImageState.Unchanged;
        StateHasChanged();
    }

    private void CatalogImageSelected(StreamContent content)
    {
        _catalogImageContent = content;
        _videoForEditVm.CatalogImageState = ImageState.Added;
        StateHasChanged();
    }

    private void CatalogImageUnSelected()
    {
        _catalogImageContent = null;
        _videoForEditVm.CatalogImageState = ImageState.Unchanged;
        StateHasChanged();
    }

    private void FeaturedCatalogImageSelected(StreamContent content)
    {
        _featuredCatalogImageContent = content;
        _videoForEditVm.FeaturedCatalogImageState = ImageState.Added;
        StateHasChanged();
    }

    private void FeaturedCatalogImageUnSelected()
    {
        _featuredCatalogImageContent = null;
        _videoForEditVm.FeaturedCatalogImageState = ImageState.Unchanged;
        StateHasChanged();
    }

    private void AnimatedGifSelected(StreamContent content)
    {
        _animatedGifContent = content;
        _videoForEditVm.AnimatedGifState = ImageState.Added;
        StateHasChanged();
    }

    private void AnimatedGifUnSelected()
    {
        _animatedGifContent = null;
        _videoForEditVm.AnimatedGifState = ImageState.Unchanged;
        StateHasChanged();
    }

    private bool HasUploadedPlayerImage()
    {
        return !string.IsNullOrWhiteSpace(_playerImageSrc);
    }

    private bool HasUploadedCatalogImage()
    {
        return !string.IsNullOrWhiteSpace(_catalogImageSrc);
    }

    private bool HasUploadedFeaturedCatalogImage()
    {
        return !string.IsNullOrWhiteSpace(_featuredCatalogImageSrc);
    }

    private bool HasUploadedAnimatedGifImage()
    {
        return !string.IsNullOrWhiteSpace(_animatedGifSrc);
    }

    private void RemovePlayerImage()
    {
        Console.WriteLine("RemovePlayerImage");

        _playerImageContent = null;
        _playerImageSrc = null;
        _videoForEditVm.PlayerImageState = ImageState.Removed;

        if (_videoForEditVm?.PlayerImage is not null)
        {
            _videoForEditVm.PlayerImage = null;
        }
        StateHasChanged();
    }

    private void RemoveCatalogImage()
    {
        _catalogImageContent = null;
        _catalogImageSrc = null;
        _videoForEditVm.CatalogImageState = ImageState.Removed;

        if (_videoForEditVm?.CatalogImage is not null)
        {
            _videoForEditVm.CatalogImage = null;

        }
        StateHasChanged();
    }

    private void RemoveFeaturedCatalogImage()
    {
        _featuredCatalogImageContent = null;
        _featuredCatalogImageSrc = null;
        _videoForEditVm.FeaturedCatalogImageState = ImageState.Removed;

        if (_videoForEditVm?.FeaturedCatalogImage is not null)
        {
            _videoForEditVm.FeaturedCatalogImage = null;
        }
        StateHasChanged();
    }

    private void RemoveAnimatedGifImage()
    {
        _animatedGifContent = null;
        _animatedGifSrc = null;
        _videoForEditVm.AnimatedGifState = ImageState.Removed;

        if (_videoForEditVm?.AnimatedGif is not null)
        {
            _videoForEditVm.AnimatedGif = null;
        }
        StateHasChanged();
    }
    #endregion Image Handler Methods

    #region Video Selection Handler Methods
    private async Task<IEnumerable<VideoItemForAutoComplete>> SearchVideos(string? value)
    {
        System.Console.WriteLine("video auto complete value: " + value ?? "value is null");

        var responseWrapper = await VideosClient.GetVideosForAutoComplete(
            new GetVideosForAutoCompleteQuery
            {
                PageNumber = 50,
                SearchText = value ?? string.Empty
            });

        //if (responseWrapper.Success)
        //{
        //    var successResult = responseWrapper.Response as SuccessResult<VideosForAutoCompleteResponse>;
        //    if (successResult != null)
        //        _videosForAutoCompleteResponse = successResult.Result;
        //}
        //else
        //{
        //    var exceptionResult = responseWrapper.Response as ExceptionResult;
        //    _serverSideValidator.Validate(exceptionResult);
        //}

        if (responseWrapper.IsSuccessStatusCode)
        {
            _videosForAutoCompleteResponse = responseWrapper.Payload;
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return _videosForAutoCompleteResponse.Videos.Items;
    }

    private IEnumerable<string> ValidateVideo(string? value)
    {
        var video = _videosForAutoCompleteResponse?.Videos?.Items.FirstOrDefault(a => a.Title.Equals(value));
        if (video is null)
        {
            yield return Resource.Video_by_that_name_not_found;
        }
    }
    #endregion

    private void SetContentAccess(int value)
    {
        Console.WriteLine($"content access: {value}");
        _videoForEditVm.ContentAccess = (ContentAccess)value;
        switch (_videoForEditVm.ContentAccess)
        {
            case ContentAccess.Free:
                _contentAccessFreeVariant = Variant.Filled;
                _contentAccessPremiumVariant = Variant.Outlined;
                _contentAccessFreeIconColor = Color.Tertiary;
                _contentAccessPremiumIconColor = Color.Secondary;
                break;
            case ContentAccess.Premium:
                _contentAccessPremiumVariant = Variant.Filled;
                _contentAccessFreeVariant = Variant.Outlined;
                _contentAccessPremiumIconColor = Color.Tertiary;
                _contentAccessFreeIconColor = Color.Secondary;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        StateHasChanged();
    }

    private void SetPublicationStatus(int value)
    {
        Console.WriteLine($"publication status: {value}");
        _videoForEditVm.PublicationStatus = (PublicationStatus)value;

        // TODO helper method
        switch (_videoForEditVm.PublicationStatus)
        {
            case PublicationStatus.Unpublished:
                _publicationStatusUnpublishedVariant = Variant.Filled;
                _publicationStatusPublishedVariant = Variant.Outlined;
                _publicationStatusScheduledVariant = Variant.Outlined;
                _publicationStatusUnpublishedIconColor = Color.Tertiary;
                _publicationStatusPublishedIconColor = Color.Secondary;
                _publicationStatusScheduledIconColor = Color.Secondary;
                break;
            case PublicationStatus.Published:
                _publicationStatusUnpublishedVariant = Variant.Outlined;
                _publicationStatusPublishedVariant = Variant.Filled;
                _publicationStatusScheduledVariant = Variant.Outlined;
                _publicationStatusUnpublishedIconColor = Color.Secondary;
                _publicationStatusPublishedIconColor = Color.Tertiary;
                _publicationStatusScheduledIconColor = Color.Secondary;
                break;
            case PublicationStatus.Scheduled:
                _publicationStatusUnpublishedVariant = Variant.Outlined;
                _publicationStatusPublishedVariant = Variant.Outlined;
                _publicationStatusScheduledVariant = Variant.Filled;
                _publicationStatusUnpublishedIconColor = Color.Secondary;
                _publicationStatusPublishedIconColor = Color.Secondary;
                _publicationStatusScheduledIconColor = Color.Tertiary;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
}

    private void OneTimePurchaseCheckedChanged(bool value)
    {
        _videoForEditVm.HasOneTimePurchasePrice = value;
        StateHasChanged();
        Console.WriteLine($"OneTimePurchaseCheckedChanged: {value}");
    }

    private void RentalPurchaseCheckedChanged(bool value)
    {
        _videoForEditVm.HasRentalPrice = value;
        StateHasChanged();
        Console.WriteLine($"RentalPurchaseCheckedChanged: {value}");
    }

    private string DurationSelectStringConverter(RentalDuration duration)
    {
        // TODO use Resource for localization
        string convertedValue = string.Empty;
        switch (duration)
        {
            case RentalDuration.OneDay:
                convertedValue = "1 Day";
                break;
            case RentalDuration.TwoDays:
                convertedValue = "2 Days";
                break;
            case RentalDuration.ThreeDays:
                convertedValue = "3 Days";
                break;
            case RentalDuration.FourDays:
                convertedValue = "4 Days";
                break;
            case RentalDuration.FiveDays:
                convertedValue = "5 Days";
                break;
            case RentalDuration.SixDays:
                convertedValue = "6 Days";
                break;
            case RentalDuration.OneWeek:
                convertedValue = "1 Week";
                break;
            case RentalDuration.TwoWeeks:
                convertedValue = "2 Weeks";
                break;
            case RentalDuration.OneMonth:
                convertedValue = "1 Month";
                break;
            case RentalDuration.ThreeMonths:
                convertedValue = "3 Months";
                break;
            case RentalDuration.SixMonths:
                convertedValue = "6 Months";
                break;
            case RentalDuration.OneYear:
                convertedValue = "1 Year";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(duration), duration, null);
        }

        return convertedValue;
    }

    private void UpdateRteValue(string value)
    {
        _videoForEditVm.FullDescription = value;
    }  


    private async Task SubmitForm()
    {
        // TODO guard clauses

        _editContext?.Validate();

        Console.WriteLine("SubmitForm");

        System.Console.WriteLine($"selected author id: {_selectedAuthor.Id} ");
        System.Console.WriteLine($"selected author name: {_selectedAuthor.Name} ");

        Console.WriteLine($"selected trailer video id: { _selectedTrailerVideo}");
        Console.WriteLine($"selected featured category video id: {_selectedFeaturedCategoryVideo}");

        // TODO subtitles


        // TODO 
        // add image url props to dtos/requests
        // account for remove image
        // mux web hook add default images

        Console.WriteLine($"player image state: {_videoForEditVm.PlayerImageState}");     


        _updateVideoCommand = new UpdateVideoCommand
        {
            Id = _videoForEditVm.Id,
            Title = _videoForEditVm.Title,
            FullDescription = _videoForEditVm.FullDescription,
            ShortDescription = _videoForEditVm.ShortDescription,
            AllowDownload = _videoForEditVm.AllowDownload,

            SeoTitle = _videoForEditVm.SeoTitle,
            SeoDescription = _videoForEditVm.SeoDescription,
            Slug = _videoForEditVm.Slug?.FormatSlug(),

            PlayerImageState = _videoForEditVm.PlayerImageState,
            CatalogImageState = _videoForEditVm.CatalogImageState,
            FeaturedCatalogImageState = _videoForEditVm.FeaturedCatalogImageState,
            AnimatedGifState = _videoForEditVm.AnimatedGifState,

            CategoryIds = _videoForEditVm?.CategoryIds ?? new List<Guid>(),
            AuthorId = _videoForEditVm.AuthorId,
            TrailerVideoId = _videoForEditVm.TrailerVideo?.Id,
            FeaturedCategoryVideoId =_videoForEditVm.FeaturedCategoryVideo?.Id,

            PublicationStatus = _videoForEditVm.PublicationStatus,
            ContentAccess = _videoForEditVm.ContentAccess,
            ReleasedDate = _videoForEditVm.ReleasedDate,
            ExpirationDate = _videoForEditVm.ExpirationDate,
            HasOneTimePurchasePrice = _videoForEditVm.HasOneTimePurchasePrice,
            OneTimePurchasePrice = _videoForEditVm.OneTimePurchasePrice,
            HasRentalPrice = _videoForEditVm.HasRentalPrice,
            RentalPrice = _videoForEditVm.RentalPrice,
            RentalDuration = _videoForEditVm.RentalDuration,
        };


        Console.WriteLine("category ids count : " + _updateVideoCommand?.CategoryIds?.Count());

        
        var categoryIdsList = _updateVideoCommand?.CategoryIds?.Select(c => c.ToString()).ToList();
        string categoryIdsAsStrings = categoryIdsList != null ? string.Join(',', categoryIdsList) : string.Empty;

        var userFormData = new MultipartFormDataContent
            {
                { new StringContent(_updateVideoCommand.Id.ToString() ?? string.Empty), "id" },
                { new StringContent(_updateVideoCommand.Title ?? string.Empty), "Title" },
                { new StringContent(_updateVideoCommand.FullDescription ?? string.Empty), "FullDescription" },
                { new StringContent(_updateVideoCommand.ShortDescription ?? string.Empty), "ShortDescription" },
                { new StringContent(_updateVideoCommand.AllowDownload.ToString()), "AllowDownload" },

                { new StringContent(_updateVideoCommand.SeoTitle ?? string.Empty), "SeoTitle" },
                { new StringContent(_updateVideoCommand.SeoDescription ?? string.Empty), "SeoDescription" },
                { new StringContent(_updateVideoCommand.Slug ?? string.Empty), "Slug" },

                { new StringContent(_updateVideoCommand.PlayerImageState.ToString()), "PlayerImageState" },
                { new StringContent(_updateVideoCommand.CatalogImageState.ToString()), "CatalogImageState" },
                { new StringContent(_updateVideoCommand.FeaturedCatalogImageState.ToString()), "FeaturedCatalogImageState" },
                { new StringContent(_updateVideoCommand.AnimatedGifState.ToString()), "AnimatedGifState" },
                
                { new StringContent(categoryIdsAsStrings), "CategoryIdsAsStrings" },
                { new StringContent(_updateVideoCommand.AuthorId?.ToString() ?? string.Empty), "AuthorId" },
                { new StringContent(_updateVideoCommand.TrailerVideoId?.ToString() ?? string.Empty), "TrailerVideoId" },
                { new StringContent(_updateVideoCommand.FeaturedCategoryVideoId?.ToString() ?? string.Empty), "FeaturedCategoryVideoId" },

                { new StringContent(_updateVideoCommand.PublicationStatus.ToString() ?? string.Empty), "PublicationStatus" },
                { new StringContent(_updateVideoCommand.ContentAccess.ToString() ?? string.Empty), "ContentAccess" },
                { new StringContent(_updateVideoCommand.ReleasedDate.ToString() ?? string.Empty), "ReleasedDate" },
                { new StringContent(_updateVideoCommand.ExpirationDate.ToString() ?? string.Empty), "ExpirationDate" },
                { new StringContent(_updateVideoCommand.HasOneTimePurchasePrice.ToString() ?? string.Empty), "HasOneTimePurchasePrice" },
                { new StringContent(_updateVideoCommand.OneTimePurchasePrice.ToString() ?? string.Empty), "OneTimePurchasePrice" },
                { new StringContent(_updateVideoCommand.HasRentalPrice.ToString() ?? string.Empty), "HasRentalPrice" },
                { new StringContent(_updateVideoCommand.RentalPrice.ToString() ?? string.Empty), "RentalPrice" },
                { new StringContent(_updateVideoCommand.RentalDuration.ToString() ?? string.Empty), "RentalDuration" },
            };

        if (_playerImageContent != null)
            userFormData.Add(_playerImageContent, "PlayerImage", _playerImageContent.Headers.GetValues("FileName").LastOrDefault());

        if (_catalogImageContent != null)
            userFormData.Add(_catalogImageContent, "CatalogImage", _catalogImageContent.Headers.GetValues("FileName").LastOrDefault());
        
        if (_featuredCatalogImageContent != null)
            userFormData.Add(_featuredCatalogImageContent, "FeaturedCatalogImage", _featuredCatalogImageContent.Headers.GetValues("FileName").LastOrDefault());
        
        if (_animatedGifContent != null)
            userFormData.Add(_animatedGifContent, "AnimatedGif", _animatedGifContent.Headers.GetValues("FileName").LastOrDefault());


        var responseWrapper = await VideosClient.UpdateVideo(userFormData);


        Console.WriteLine("httpResponse: " + responseWrapper);
        Console.WriteLine("httpResponse.Success: " + responseWrapper.IsSuccessStatusCode);


        //if (httpResponse.Success)
        //{
        //    var successResult = httpResponse.Response as SuccessResult<string>;
        //    Snackbar?.Add(successResult?.Result, Severity.Success);
        //    NavigationManager?.NavigateTo("content/videos");
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
            NavigationManager.NavigateTo("content/videos");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }


    #region Auto Complete Handlers
    private async Task<IEnumerable<CategoryItemForAutoComplete>> SearchCategories(string? value)
    {
        System.Console.WriteLine("category auto complete value: " + value ?? "value is null");

        var responseWrapper = await CategoriesClient.GetCategoriesForAutoComplete(
            new GetCategoriesForAutoCompleteQuery
            {
                PageNumber = 50,
                SearchText = value ?? string.Empty
            });

        if (responseWrapper.IsSuccessStatusCode)
        {
            _categoriesForAutoResponse = responseWrapper.Payload;
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return _categoriesForAutoResponse.Categories.Items;
    }

    private void CategorySelectedHandler(CategoryItemForAutoComplete value)
    {
        _selectedCategory = value;
        if (_selectedCategories is null)
        {
            _selectedCategories = new List<CategoryItemForAutoComplete>();
        }

        if (_videoForEditVm?.CategoryIds is null)
        {
            _videoForEditVm.CategoryIds = new List<Guid>();
        }

        if (!_selectedCategories.Contains(value, new CategoryItemForAutoCompleteEqualityComparer()))
        {
            Console.WriteLine($"add to categories: {value.Title}");
            _selectedCategories.Add(value);
            if (!_videoForEditVm.CategoryIds.Contains(value.Id))
            {
                _videoForEditVm.CategoryIds.Add(value.Id);
            }
        }
        StateHasChanged();
    }

    private void CategoryRemovedHandler(MudChip chip)
    {
        var categoryToRemove = _selectedCategories.FirstOrDefault(c => c.Title.Equals(chip.Text));
        _selectedCategories.Remove(categoryToRemove);
        _videoForEditVm?.CategoryIds?.Remove(categoryToRemove.Id);
        StateHasChanged();
    }

    private IEnumerable<string> ValidateCategory(string? value)
    {
        var category = _categoriesForAutoResponse?.Categories?.Items.FirstOrDefault(a => a.Title.Equals(value));
        if (category is null)
        {
            yield return "Category_by_that_name_not_found";
        }
    }

    private void AuthorSelectedHandler(AuthorItemForAutoComplete value)
    {
        _selectedAuthor = value;
        if (!_selectedAuthor.Equals(Guid.Empty)) 
        {
            _videoForEditVm.AuthorId = _selectedAuthor.Id;
        }
        StateHasChanged();
    }
    
    private async Task<IEnumerable<AuthorItemForAutoComplete>> SearchAuthors(string? value)
    {
        System.Console.WriteLine("author auto complete value: " + value ?? "value is null");

        var responseWrapper = await AuthorsClient.GetAuthorsForAutoComplete(
            new GetAuthorsForAutoCompleteQuery{
            PageNumber = 50,
            SearchText = value ?? string.Empty
        });

         if (responseWrapper.IsSuccessStatusCode)
        {
            _authorsForAutoResponse = responseWrapper.Payload;
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return _authorsForAutoResponse.Authors.Items;        
    }

    private IEnumerable<string> ValidateAuthor(string? value)
    {
        var author = _authorsForAutoResponse?.Authors?.Items.FirstOrDefault(a => a.Name.Equals(value));
        if (author is null)
        {
            yield return Resource.Author_by_that_name_not_found;
        }
    }
    #endregion Auto Complete Handlers

    #endregion Private Methods
}
