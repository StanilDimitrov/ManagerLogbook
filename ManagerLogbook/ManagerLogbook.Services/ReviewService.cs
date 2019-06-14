using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
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

        public async Task<ReviewDTO> CreateReviewAsync(string originalDescription, int businessUnitId, int rating)
        {
            businessValidator.IsDescriptionInRange(originalDescription);
            businessValidator.IsRatingInRange(rating);

            //automatic edit 
            var editedDescription = reviewEditor.AutomaticReviewEditor(originalDescription);

            //check visibility
            var checkVisibility = reviewEditor.CheckReviewVisibility(editedDescription);

            var review = new Review()
            {
                OriginalDescription = originalDescription,
                EditedDescription = editedDescription,
                Rating = rating,
                CreatedOn = DateTime.Now,
                isVisible = checkVisibility,
                BusinessUnitId = businessUnitId
            };

            this.context.Reviews.Add(review);

            await this.context.SaveChangesAsync();

            var result = await this.context.Reviews
                                           .Include(bu => bu.BusinessUnit)
                                           .FirstOrDefaultAsync(x => x.Id == review.Id);

            return result.ToDTO();
        }

        public async Task<ReviewDTO> UpdateReviewAsync(int reviewId, string editedDescription)
        {
            var review = await this.context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                throw new NotFoundException(ServicesConstants.ReviewNotFound);
            }

            businessValidator.IsDescriptionInRange(editedDescription);

            review.EditedDescription = editedDescription;

            await this.context.SaveChangesAsync();

            var result = await this.context.Reviews
                                           .Include(bu => bu.BusinessUnit)
                                           .FirstOrDefaultAsync(x => x.Id == review.Id);

            return result.ToDTO();
        }

        public async Task<ReviewDTO> MakeVisibleReviewAsync(int reviewId)
        {
            var review = await this.context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                throw new NotFoundException(ServicesConstants.ReviewNotFound);
            }

            review.isVisible = true;

            await this.context.SaveChangesAsync();

            var result = await this.context.Reviews
                                           .Include(bu => bu.BusinessUnit)
                                           .FirstOrDefaultAsync(x => x.Id == review.Id);

            return result.ToDTO();
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsByBusinessUnitIdAsync(int businessUnitId)
        {
            var businessUnit = await this.context.BusinessUnits.FindAsync(businessUnitId);

            if (businessUnit == null)
            {
                throw new NotFoundException(ServicesConstants.BusinessUnitNotFound);
            }

            var result = await this.context.Reviews
                                    .Where(bu => bu.BusinessUnitId == businessUnitId)
                                    .Where(co => co.isVisible == true)
                                    .Include(bu => bu.BusinessUnit)
                                    .OrderByDescending(co => co.CreatedOn)
                                    .Select(r => r.ToDTO())
                                    .ToListAsync();

            return result;
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsByModeratorIdAsync(string moderatorId)
        {
            var userModerator = await this.context.Users.FindAsync(moderatorId);

            if (userModerator == null)
            {
                throw new NotFoundException(ServicesConstants.UserNotFound);
            }

            var result = await this.context.Reviews
                                    .Where(bu => bu.BusinessUnitId == userModerator.BusinessUnitId)
                                    .Include(bu => bu.BusinessUnit)
                                    .OrderByDescending(co => co.CreatedOn)
                                    .Select(r => r.ToDTO())
                                    .ToListAsync();

            return result;
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsByDateAsync(DateTime date)
        {
            businessValidator.IsDateValid(date);

            var result = await this.context.Reviews
                                     .Where(bu => bu.CreatedOn == date)
                                     .Where(co => co.isVisible == true)
                                     .Include(bu => bu.BusinessUnit)
                                     .OrderByDescending(co => co.CreatedOn)
                                     .Select(r => r.ToDTO())
                                     .ToListAsync();

            return result;
        }

        public async Task<ICollection<ReviewDTO>> GetAllReviewsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            businessValidator.IsDateValid(startDate);
            businessValidator.IsDateValid(endDate);

            startDate = startDate < endDate ? startDate : endDate;
            endDate = endDate > startDate ? endDate : startDate;

            var result = await this.context.Reviews
                                     .Where(bu => bu.CreatedOn >= startDate && bu.CreatedOn <= endDate)
                                     .Where(co => co.isVisible == true)
                                     .Include(bu => bu.BusinessUnit)
                                     .OrderByDescending(co => co.CreatedOn)
                                     .Select(r => r.ToDTO())
                                     .ToListAsync();

            return result;
        }

        public async Task<ReviewDTO> GetReviewByIdAsync(int reviewId)
        {
            var result = await this.context.Reviews
                                    .Include(bu => bu.BusinessUnit)
                                    .FirstOrDefaultAsync(r => r.Id == reviewId);
                                    
            if (result == null)
            {
                throw new NotFoundException(ServicesConstants.ReviewNotFound);
            }

            return result.ToDTO();

        }
    }
}
