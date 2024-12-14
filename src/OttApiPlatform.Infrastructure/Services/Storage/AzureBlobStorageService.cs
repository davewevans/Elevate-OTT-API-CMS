
using Azure.Core;
using Azure.Storage.Sas;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetSasToken;
using System.ComponentModel;

namespace OttApiPlatform.Infrastructure.Services.Storage;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    #region Private Fields

    private readonly string _connectionString;
    private readonly IUtcTimeService _utcTimeService;
    #endregion Private Fields

    #region Public Constructors

    public AzureBlobStorageService(IConfiguration configuration, IUtcTimeService utcTimeService)
    {
        _connectionString = configuration.GetConnectionString("AzureStorageConnection");
        _utcTimeService = utcTimeService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<FileMetaData> UploadFile(IFormFile formFile,
                                               string directoryPath,
                                               bool fileRenameAllowed = false,
                                               string baseUrl = null,
                                               CancellationToken cancellationToken = default)
    {
        // Check if file is null or empty.
        if (formFile is not { Length: > 0 })
            return null;

        try
        {
            // Generate a unique file name using a Unix Time.
            var fileName = $"{_utcTimeService.GetUnixTimeMilliseconds()}";

            // Rename file if allowed.
            if (fileRenameAllowed)
                fileName = $"{formFile.FileName.ToUrlFriendlyString()}-{fileName}";

            // Append file extension to the generated file name.
            fileName = $"{fileName}{Path.GetExtension(formFile.FileName)}";

            // Create a new BlobContainerClient instance with the specified connection string and
            // directory path.
            var containerClient = new BlobContainerClient(_connectionString, directoryPath.Split("/")[0]);

            // Create the container if it doesn't exist.
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            // Set the access policy to allow public access to blobs within the container.
            await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);

            // Get a reference to the blob client with the specified file name.
            var blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file to the blob storage.
            await blobClient.UploadAsync(formFile.OpenReadStream(), cancellationToken: cancellationToken);

            // Return the meta data of the uploaded file.
            return new FileMetaData
            {
                FileUri = blobClient.Uri.ToString(),
                FileName = fileName
            };
        }
        catch
        {
            // Throw an exception if the file upload fails.
            throw new Exception(Resource.File_has_not_been_uploaded);
        }
    }

    public async Task<List<FileMetaData>> UploadMultipleFiles(IList<IFormFile> formFiles,
                                                              string directoryPath,
                                                              bool fileRenameAllowed = false,
                                                              int defaultFileIndex = 0,
                                                              string baseUrl = null,
                                                              CancellationToken cancellationToken = default)
    {
        var filePaths = new List<FileMetaData>();

        // If there are no form files to upload, return an empty list.
        if (formFiles == null || formFiles.Count == 0)
            return new List<FileMetaData>();

        // Upload each form file to the specified blob storage container.
        foreach (var formFile in formFiles.Select((value, index) => new { Index = index, Value = value }))
            if (formFile.Value.Length > 0)
                try
                {
                    // Generate a unique file name using a Unix Time.
                    var fileName = $"{_utcTimeService.GetUnixTimeMilliseconds()}";

                    // Rename the uploaded file if necessary.
                    if (fileRenameAllowed)
                        fileName = $"{formFile.Value.FileName.ToUrlFriendlyString()}-{fileName}";

                    fileName = $"{fileName}{Path.GetExtension(formFile.Value.FileName)}";

                    // Create a new blob container client and upload the file to the container.
                    var containerClient = new BlobContainerClient(_connectionString, directoryPath);
                    await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
                    await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);
                    var blobClient = containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(formFile.Value.OpenReadStream(), cancellationToken: cancellationToken);

                    // Add metadata for the uploaded file to the list of file paths.
                    filePaths.Add(new FileMetaData
                    {
                        FileName = fileName,
                        FileUri = blobClient.Uri.ToString(),
                        IsDefault = defaultFileIndex == formFile.Index
                    });
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            else
                // Throw an exception if the form file is empty.
                throw new Exception(Resource.File_is_empty);

        return filePaths;
    }

    public async Task<FileMetaData> EditFile(IFormFile formFile,
                                             string directoryPath,
                                             string oldFileUri,
                                             string rootFolderName = default,
                                             CancellationToken cancellationToken = default)
    {
        // If no new file was provided, return the URI of the old file through FileMetaData object.
        if (formFile == null)
            return new FileMetaData
            {
                FileUri = oldFileUri,
            };

        // If an old file URI was provided, delete the old file.
        if (!string.IsNullOrEmpty(oldFileUri))
            await DeleteFileIfExists(oldFileUri, rootFolderName);

        // Upload the new file.
        return await UploadFile(formFile, directoryPath, cancellationToken: cancellationToken);
    }

    public async Task DeleteFileIfExists(string fileUri, string containerName = default)
    {
        // Check if the file URI is not empty or null.
        if (!string.IsNullOrEmpty(fileUri))
        {
            // Create a BlobContainerClient with the connection string and the container name.
            var container = new BlobContainerClient(_connectionString, containerName);

            // Check if the container exists.
            if (await container.ExistsAsync())
            {
                // Set the access policy of the container to PublicAccessType.BlobContainer.
                await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

                // Split the file URI into an array and get the last element (which should be the
                // file name).
                var fileUriArray = fileUri.Split('/').ToArray();
                var fileName = fileUriArray.LastOrDefault();

                // Delete the blob (file) if it exists, including any snapshots.
                await container.DeleteBlobIfExistsAsync(fileName, DeleteSnapshotsOption.IncludeSnapshots);
            }
        }
    }

    public FileStatus GetFileState(IFormFile formFile, string oldUrl)
    {
        // If the IFormFile object is not null or has a length greater than 0, the file is modified.
        if (formFile is not null or { Length: > 0 })
            return FileStatus.Modified;

        // If the old URL is not null or whitespace, the file is unchanged. Otherwise, it's deleted.
        return !string.IsNullOrWhiteSpace(oldUrl) ? FileStatus.Unchanged : FileStatus.Deleted;
    }

    public SasTokenResponse GetSasTokenForVideoContainer(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient("videos");

        // Generate unique file name
        var timestamp = $"{_utcTimeService.GetUnixTimeMilliseconds()}";
        var uniqueFileName = $"{ fileName.ToUrlFriendlyString() }-{ timestamp }";

        var blobClient = containerClient.GetBlobClient(uniqueFileName);

        var blobSasBuilder = new BlobSasBuilder
        {
            BlobContainerName = "videos",
            BlobName = uniqueFileName,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(2),
        };

        blobSasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Read | BlobSasPermissions.Create);

        var sasToken = blobClient.GenerateSasUri(blobSasBuilder);

        return new SasTokenResponse
        {
            UploadUrl = sasToken.ToString(),
            UniqueFileName = uniqueFileName,
            AccountName = containerClient.AccountName,
            ContainerName = "videos",
            ContainerUri = containerClient.Uri,
            SASUri = sasToken,
            SASToken = sasToken.Query,
            SASExpireOnInMinutes = 120
        };
    }

    #endregion Public Methods

    #region Private Methods

    private static async Task DeleteBlobIfExistsAsync(BlobContainerClient container, string prefix, int level)
    {
        // Use GetBlobsByHierarchyAsync method to get a page of blobs with the given prefix, using
        // '/' as the delimiter.
        await foreach (var page in container.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/").AsPages())
        {
            // Iterate through the blobs in the page and delete them one by one using the
            // DeleteBlobIfExistsAsync method of the container.
            foreach (var blob in page.Values.Where(item => item.IsBlob).Select(item => item.Blob))
                await container.DeleteBlobIfExistsAsync(blob.Name);

            // Get the prefixes in the page and recursively call the DeleteBlobIfExistsAsync method
            // on them to delete all blobs under that prefix.
            var prefixes = page.Values.Where(item => item.IsPrefix).Select(item => item.Prefix);
            foreach (var p in prefixes)
                await DeleteBlobIfExistsAsync(container, p, level + 1);
        }
    }

    #endregion Private Methods
}