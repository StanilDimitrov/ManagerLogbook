using Microsoft.AspNetCore.Http;

namespace ManagerLogbook.Web.Services.Contracts
{
    public interface IImageOptimizer
    {
        string OptimizeImage(IFormFile inputImage, int endHeight, int endWidth);

        void DeleteOldImage(string imageName);
    }
}
