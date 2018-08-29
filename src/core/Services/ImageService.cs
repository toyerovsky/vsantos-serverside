using System;
using System.IO;
using System.Threading.Tasks;
using VRP.BLL.Dto;

namespace VRP.BLL.Services
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// A method to upload an image to server
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>File path</returns>
        public async Task<string> UploadImageAsync(ImageDto dto)
        {
            string filePath = $"{Environment.CurrentDirectory}/{Guid.NewGuid()}.jpg";
            byte[] image = new byte[1048576];
            if (Convert.TryFromBase64String(dto.ImageBase64, image, out int bytesWritten))
            {
                using (FileStream file = File.Create(filePath))
                {
                    await file.WriteAsync(image, 0, 1048576);
                }
                return filePath;
            }

            throw new FormatException("Provided Base64 of an image is incorrect.");
        }
    }
}