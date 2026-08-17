using GiftShop.Services.Interfaces;

namespace GiftShop.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> UploadImageAsync(IFormFile? file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "images",
                folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string extension = Path.GetExtension(file.FileName);

            string fileName =
                Guid.NewGuid().ToString() + extension;

            string filePath =
                Path.Combine(uploadsFolder, fileName);

            using FileStream stream = new(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return fileName;
        }

        public void DeleteImage(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string path = Path.Combine(
                _environment.WebRootPath,
                "images",
                folderName,
                fileName);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}