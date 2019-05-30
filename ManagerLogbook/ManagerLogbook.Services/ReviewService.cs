using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ManagerLogbookContext context;
        private readonly IBusinessValidator businessValidator;
        private readonly IReviewEditor reviewEditor;

        public ReviewService(ManagerLogbookContext context,
                             IBusinessValidator businessValidator,
                             IReviewEditor reviewEditor)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.businessValidator = businessValidator ?? throw new ArgumentNullException(nameof(businessValidator));
            this.reviewEditor = reviewEditor ?? throw new ArgumentNullException(nameof(reviewEditor));
        }

        public async Task<ReviewDTO> CreateReviewDTOAsync(string originalDescription, int rating, int businessUnitId)
        {
            businessValidator.IsDescriptionInRange(originalDescription);
            businessValidator.IsRatingInRange(rating);

            //automatic edit 
            var editedDescription = reviewEditor.AutomaticReviewEditor(originalDescription);

            //da li da se podava kato dependency DateTime v constructora?

            var review = new Review()
            {
                OriginalDescription = originalDescription,
                EditedDescription = editedDescription,
                Rating = rating,
                CreatedOn = DateTime.Now,
                isVisible = false,
                BusinessUnitId = businessUnitId
            };

            this.context.Reviews.Add(review);

            await this.context.SaveChangesAsync();

            var result = await this.context.Reviews
                                           .Include(bu => bu.BusinessUnit)
                                           .FirstOrDefaultAsync(x => x.Id == review.Id);

            return result.ToDTO();
        }

        public async Task<ReviewDTO> UpdateReviewDTOAsync(int reviewId, string editedDescription)
        {
            var review = await this.context.Reviews.FindAsync(reviewId);

            businessValidator.IsDescriptionInRange(editedDescription);

            review.EditedDescription = editedDescription;

            await this.context.SaveChangesAsync();

            var result = await this.context.Reviews
                                           .Include(bu => bu.BusinessUnit)
                                           .FirstOrDefaultAsync(x => x.Id == review.Id);

            return result.ToDTO();
        }

        public async Task<ReviewDTO> MakeVisibleReviewDTOAsync(int reviewId)
        {
            var review = await this.context.Reviews.FindAsync(reviewId);

            review.isVisible = true;

            await this.context.SaveChangesAsync();

            var result = await this.context.Reviews
                                           .Include(bu => bu.BusinessUnit)
                                           .FirstOrDefaultAsync(x => x.Id == review.Id);

            return result.ToDTO();
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsDTOByBusinessUnitIdAsync(int businessUnitId)
        {
            var result = await this.context.Reviews
                                    .Where(bu => bu.BusinessUnitId == businessUnitId)
                                    .Include(bu => bu.BusinessUnit)
                                    .Select(r => r.ToDTO())
                                    .ToListAsync();

            return result;
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsDTOByModeratorIdAsync(string moderatorId)
        {
            var userModerator = await this.context.Users.FindAsync(moderatorId);

            var result = await this.context.Reviews
                                    .Where(bu => bu.BusinessUnitId == userModerator.BusinessUnitId)
                                    .Include(bu => bu.BusinessUnit)
                                    .Select(r => r.ToDTO())
                                    .ToListAsync();

            return result;
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsDTOByDateAsync(DateTime date)
        {
            var result = await this.context.Reviews
                                     .Where(bu => bu.CreatedOn == date)
                                     .Include(bu => bu.BusinessUnit)
                                     .Select(r => r.ToDTO())
                                     .ToListAsync();

            return result;
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsDTOInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var result = await this.context.Reviews
                                     .Where(bu => bu.CreatedOn >= startDate && bu.CreatedOn <= endDate)
                                     .Include(bu => bu.BusinessUnit)
                                     .Select(r => r.ToDTO())
                                     .ToListAsync();

            return result;
        }
    }
}
