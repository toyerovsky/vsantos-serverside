using System.Threading.Tasks;
using VRP.BLL.Dto;

namespace VRP.BLL.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(ImageDto dto);
    }
}