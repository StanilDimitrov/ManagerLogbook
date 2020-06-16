using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(ReviewModel model);
        
        Task<ReviewDTO> UpdateReviewAsync(ReviewModel model);

        Task<ReviewDTO> MakeReviewInvisibleAsync(int reviewId);

        Task<IReadOnlyCollection<ReviewDTO>> GetReviewsByBusinessUnitAsync(int businessUnitId);
        
        Task<IReadOnlyCollection<ReviewDTO>> GetReviewsByModeratorAsync(string moderatorId);
    }
}
