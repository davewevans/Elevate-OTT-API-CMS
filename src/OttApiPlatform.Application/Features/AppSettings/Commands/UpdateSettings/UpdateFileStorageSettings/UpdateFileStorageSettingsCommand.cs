namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateFileStorageSettings;

public class UpdateFileStorageSettingsCommand : IRequest<Envelope<FileStorageSettingsResponse>>
{
    #region Public Properties

    public string Id { get; set; }
    public int StorageType { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(FileStorageSettings fileStorageSettings)
    {
        fileStorageSettings.Id = Guid.Parse(Id);
        fileStorageSettings.StorageType = (StorageTypes)Enum.Parse(typeof(StorageTypes), StorageType.ToString(), true);
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateFileStorageSettingsCommandHandler : IRequestHandler<UpdateFileStorageSettingsCommand, Envelope<FileStorageSettingsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IConfigReaderService _configReaderService;

        #endregion Private Fields

        #region Public Constructors

        public UpdateFileStorageSettingsCommandHandler(IApplicationDbContext dbContext, IConfigReaderService configReaderService)
        {
            _dbContext = dbContext;
            _configReaderService = configReaderService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<FileStorageSettingsResponse>> Handle(UpdateFileStorageSettingsCommand request, CancellationToken cancellationToken)
        {
            // Parse file storage settings ID from request.
            if (!Guid.TryParse(request.Id, out var fileStorageSettingsId))
                return Envelope<FileStorageSettingsResponse>.Result.BadRequest(Resource.Invalid_file_storage_Id);

            // Get the file storage settings entity from the database, or create a new one from the
            // app settings.
            var fileStorageSettings = await _dbContext.FileStorageSettings.FirstOrDefaultAsync(fs => fs.Id == fileStorageSettingsId, cancellationToken: cancellationToken)
                                      ?? _configReaderService.GetAppFileStorageOptions().MapToEntity();

            // Map the request data to the entity.
            request.MapToEntity(fileStorageSettings);

            // Update the entity in the database.
            _dbContext.FileStorageSettings.Update(fileStorageSettings);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create a response containing the ID of the updated file storage settings and a
            // success message.
            var tokenSettingsResponse = new FileStorageSettingsResponse
            {
                Id = fileStorageSettings.Id,
                SuccessMessage = Resource.File_storage_settings_have_been_updated_successfully
            };
            return Envelope<FileStorageSettingsResponse>.Result.Ok(tokenSettingsResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}