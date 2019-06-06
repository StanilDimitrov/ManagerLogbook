using ManagerLogbook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(string originalDescription, int rating, int businessUnitId);
        
        Task<ReviewDTO> UpdateReviewAsync(int reviewId, string editedDescription);

        Task<ReviewDTO> MakeVisibleReviewAsync(int reviewId);

        Task<ICollection<ReviewDTO>> GetAllReviewsByBusinessUnitIdAsync(int businessUnitId);
        
        Task<ICollection<ReviewDTO>> GetAllReviewsByModeratorIdAsync(string moderatorId);

        Task<ICollection<ReviewDTO>> GetAllReviewsByDateAsync(DateTime date);

        Task<ICollection<ReviewDTO>> GetAllReviewsInDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
