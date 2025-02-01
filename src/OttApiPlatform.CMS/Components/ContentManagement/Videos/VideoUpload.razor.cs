using System;
using System.ComponentModel;
using System.Diagnostics;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Commands.CreateVideo;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetNewStorageName;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetSasToken;
using OttApiPlatform.CMS.Models.Videos;
using static System.Net.WebRequestMethods;

namespace OttApiPlatform.CMS.Components.ContentManagement.Videos;

public partial class VideoUpload : ComponentBase, IDisposable
{
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IVideoUploadClient VideoUploadClient { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Parameter]
    public EventCallback OnVideoUploadComplete { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }

    private SasTokenResponse SasTokenResponse { get; set; }
    private NewStorageNameResponse NewStorageNameResponse { get; set; }

    private List<UploadFileModel> _filesToUpload = new ();

    private string _acceptedFileExtensions = ".mp4, .mkv, .mov, .avi, .wmv";

    private int _maximumFileCount = 10;

    private bool _uploadInProgress = false;

    private bool _showMaxNumberReachedAlert = false;

    private bool _clearing = false;
    private static string _defaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = _defaultDragClass;

    private CancellationTokenSource _cts;

    private UploadProgressModel _uploadProgress;

    IReadOnlyList<IBrowserFile> _files = null;


    private void OnInputFileChanged(InputFileChangeEventArgs e)
    {
        _showMaxNumberReachedAlert = false;

        ClearDragClass();
        
        try
        {
            _files = e.GetMultipleFiles(maximumFileCount: _maximumFileCount);
        }
        catch (InvalidOperationException ex)
        {
            System.Console.WriteLine(ex.Message);
            _showMaxNumberReachedAlert = true;
        }

        System.Console.WriteLine("_files count: " + _files?.Count());

        if (_files is null) return;

        if (_files.Count() > _maximumFileCount)
        {
            _showMaxNumberReachedAlert = true;
            StateHasChanged();
        }


        foreach (var file in _files)
        {
            if (_filesToUpload?.Count() < _maximumFileCount)
            {
                _filesToUpload?.Add(
                    new UploadFileModel
                    {
                        BrowserFile = file,
                        FileName = file.Name,
                        FileSize = file.Size,
                        Extension = Path.GetExtension(file.Name),
                        ContentType = file.ContentType,
                        MaxSizeAllowed = 53687091200, // 50GB
                    }
                );
            }
            else
            {
                _showMaxNumberReachedAlert = true;
            }
        }
    }

    private async Task Clear()
    {
        _clearing = true;
        _filesToUpload?.Clear();
        ClearDragClass();
        await Task.Delay(100);
        _clearing = false;
        _showMaxNumberReachedAlert = false;
    }

    private async Task HandleFileUpload()
    {
        if (_files == null) return;

        _uploadInProgress = true;
        foreach (var file in _filesToUpload)
        {
            var sasTokenResponse = await VideoUploadClient.GetAzureBlobSasToken(file.BrowserFile.Name);
            _cts = new CancellationTokenSource();

            file.UploadProgress = UploadProgressModel.CreateUploadProgress();
            if (file.UploadProgress != null)
            {
                file.UploadProgress.Maximum = 100;
                file.UploadProgress.PropertyChanged += ProgressValueChangedHandler;
            }

            await VideoUploadClient.UploadWithAzureBlobSasToken(sasTokenResponse.Payload.SASUri, file, _cts.Token);

            // TODO create new asset

        }

        _filesToUpload?.Clear();
        _uploadInProgress = false;

        ShowSnackbar("Upload complete!", Severity.Success);
    }

    private void ShowSnackbar(string message, Severity level)
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        Snackbar.Add(message, level);
    }

    private void ProgressValueChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }

    private void CancelUpload()
    {
        _cts?.Cancel();
    }

    private void SetDragClass()
    {
        _dragClass = $"{_defaultDragClass} mud-border-primary";
    }

    private void ClearDragClass()
    {
        _dragClass = _defaultDragClass;
    }

    private string StripHyphensFromGuid(Guid guid)
    {
        return guid.ToString().Replace("-", "");
    }

    public void Dispose()
    {
        _cts?.Dispose();
        Snackbar?.Dispose();
    }
}

