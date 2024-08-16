namespace OttApiPlatform.Application.Features.FileUpload;

public class FileUploadCommand : IRequest<Envelope<FileUploadResponse>>
{
    #region Public Properties

    public IFormFile File { get; set; }
    public bool FileRenameAllowed { get; set; }

    #endregion Public Properties

    public class UploadUserProfilePictureCommandHandler : IRequestHandler<FileUploadCommand, Envelope<FileUploadResponse>>
    {
        #region Private Fields

        private readonly IStorageProvider _storageProvider;

        #endregion Private Fields

        #region Public Constructors

        public UploadUserProfilePictureCommandHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        #region Public Methods

        public async Task<Envelope<FileUploadResponse>> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            // while the cancellation token has not been triggered.
            while (!cancellationToken.IsCancellationRequested)
            {
                // get the storage service instance.
                var storageService = await _storageProvider.InvokeInstanceAsync();

                // upload the file using the storage service instance.
                var fileMetaData = await storageService.UploadFile(request.File, "users", request.FileRenameAllowed,null, cancellationToken);

                // create a new response object.
                var uploadFileResponse = new FileUploadResponse
                {
                    FileUri = fileMetaData.FileUri,
                };

                // return the response object wrapped in an envelope with a success status.
                return Envelope<FileUploadResponse>.Result.Ok(uploadFileResponse);
            }

            // return a failure status with a cancellation message.
            return Envelope<FileUploadResponse>.Result.BadRequest("File upload was cancelled by the user.");
        }

        #endregion Public Methods
    }

    #endregion Public Constructors
}