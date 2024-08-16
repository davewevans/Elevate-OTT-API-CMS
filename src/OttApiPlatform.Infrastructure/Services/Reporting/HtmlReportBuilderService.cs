using IronPdf;
using System.Net.Http;

namespace OttApiPlatform.Infrastructure.Services.Reporting;

public class HtmlReportBuilderService : IHtmlReportBuilderService
{
    #region Private Fields

    private readonly IWebHostEnvironment _env;
    private readonly IUtcTimeService _utcTimeService;
    private readonly IStorageProvider _storageProvider;

    #endregion Private Fields

    #region Public Constructors

    public HtmlReportBuilderService(IWebHostEnvironment env, IUtcTimeService utcTimeService, IStorageProvider storageProvider)
    {
        _env = env;
        _utcTimeService = utcTimeService;
        _storageProvider = storageProvider;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<FileMetaData> GenerateApplicantsPdfFromHtml(IReadOnlyList<ApplicantItem> applicantItems, string baseUrl = null)
    {
        // Read the HTML template from a file.
        var templateFile = await File.ReadAllTextAsync(GetPath("templates", "applicants.html"));

        // Build a Stubble template engine.
        var builder = new StubbleBuilder();

        // Render the template using the Stubble template engine and the applicantItems.
        var html = await builder.Build().RenderAsync(templateFile, new { Applicants = applicantItems, Logo = "Logo" });

        // Create a new filename for the HTML file.
        var htmlFileName = $"{_utcTimeService.GetUnixTimeMilliseconds()}.html";

        // Convert the HTML content to a FormFile.
        var htmlFormFile = ConvertHtmlToFormFile(html, htmlFileName);

        // Get the storage service instance.
        var storageService = await _storageProvider.InvokeInstanceAsync();

        // Upload the HTML file using the storage service instance.
        var fileMetaData = await storageService.UploadFile(htmlFormFile, "reports/HTML", fileRenameAllowed: false, baseUrl: baseUrl);

        // Render the HTML file as a PDF and upload the PDF to the storage service instance.
        var pdfFile = await RenderAndUploadAsPdf(fileMetaData.FileUri, fileMetaData.FileName, baseUrl);

        return pdfFile;
    }

    public string GetPath(string containerName, string fileName)
    {
        // Combine the containerName and fileName to get the physical path to the file.
        return Path.Combine(_env.WebRootPath, Path.Combine(containerName, fileName));
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Converts HTML content to an <see cref="IFormFile"/>.
    /// </summary>
    /// <param name="html">The HTML content to convert.</param>
    /// <param name="htmlFileName">The file name to assign to the converted form file.</param>
    /// <returns>The converted <see cref="IFormFile"/>.</returns>
    private IFormFile ConvertHtmlToFormFile(string html, string htmlFileName)
    {
        // Convert HTML content to a memory stream.
        var bytes = Encoding.UTF8.GetBytes(html);
        var memoryStream = new MemoryStream(bytes);

        // Create an IFormFile from the memory stream.
        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "text/html", htmlFileName);

        return formFile;
    }

    /// <summary>
    /// Converts a PDF stream to an <see cref="IFormFile"/>.
    /// </summary>
    /// <param name="pdfStream">The PDF stream to convert.</param>
    /// <param name="pdfFileName">The file name to assign to the converted form file.</param>
    /// <returns>The converted <see cref="IFormFile"/>.</returns>
    private IFormFile ConvertPdfToFormFile(MemoryStream pdfStream, string pdfFileName)
    {
        // Create an IFormFile from the PDF stream.
        var formFile = new FormFile(pdfStream, 0, pdfStream.Length, "application/pdf", pdfFileName);

        return formFile;
    }

    /// <summary>
    /// Renders a PDF file from a URL using ChromePdfRenderer and upload it to the storage service instance.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to render as a PDF.</param>
    /// <param name="pdfFileName">The file name to assign to the generated PDF.</param>
    /// <param name="baseUrl">The base URL for generating the PDF file.</param>
    /// <returns>The metadata for the generated PDF file.</returns>
    private async Task<FileMetaData> RenderAndUploadAsPdf(string fileUrl, string pdfFileName, string baseUrl = null)
    {
        // Create a new instance of ChromePdfRenderer.
        var renderer = new ChromePdfRenderer();

        // Download the HTML content from the file URL using HttpClient
        string htmlContent;
        using (var httpClient = new HttpClient())
            htmlContent = await httpClient.GetStringAsync(fileUrl);

        // Use ChromePdfRenderer to generate a PDF from the specified URL.
        using var pdf = renderer.RenderHtmlAsPdf(htmlContent);

        // Save the PDF file to the specified physical file path.
        var pdfFormFile = ConvertPdfToFormFile(pdf.Stream, pdfFileName.Replace(".html", ".pdf"));

        // get the storage service instance.
        var storageService = await _storageProvider.InvokeInstanceAsync();

        // upload the file using the storage service instance.
        var fileMetaData = await storageService.UploadFile(pdfFormFile, "reports/PDF", fileRenameAllowed: false, baseUrl: baseUrl);

        // Return the metadata for the generated PDF file.
        return new FileMetaData { FileName = fileMetaData.FileName, FileUri = fileMetaData.FileUri, ContentType = MediaTypeNames.Application.Pdf };
    }

    #endregion Private Methods
}