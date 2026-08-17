using Microsoft.AspNetCore.Http;

namespace GiftShop.Services.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile? file, string folderName);

        void DeleteImage(string folderName, string fileName);
    }
}