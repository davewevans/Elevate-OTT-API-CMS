namespace OttApiPlatform.Application.Common.Contracts;

public interface IImageOptimizerService
{
    Task<byte[]> TinifyImage(IFormFile imageFile);
}
