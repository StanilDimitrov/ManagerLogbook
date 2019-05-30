using ManagerLogbook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagerLogbook.Services.Contracts
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewDTOAsync(string originalDescription, int rating, int businessUnitId);
        
        Task<ReviewDTO> UpdateReviewDTOAsync(int reviewId, string editedDescription);

        Task<ReviewDTO> MakeVisibleReviewDTOAsync(int reviewId);

        Task<ICollection<ReviewDTO>> GetAllReviewsDTOByBusinessUnitIdAsync(int businessUnitId);
        
        Task<ICollection<ReviewDTO>> GetAllReviewsDTOByModeratorIdAsync(string moderatorId);

        Task<ICollection<ReviewDTO>> GetAllReviewsDTOByDateAsync(DateTime date);

        Task<ICollection<ReviewDTO>> GetAllReviewsDTOInDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
