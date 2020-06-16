using ManagerLogbook.Data;
using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.CustomExeptions;
using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Services.Mappers;
using ManagerLogbook.Services.Models;
using ManagerLogbook.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ManagerLogbookContext _context;
        private readonly IBusinessValidator _businessValidator;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly IUserService _userService;
        private readonly IReviewEditor _reviewEditor;

        public ReviewService(ManagerLogbookContext context,
                             IBusinessValidator businessValidator,
                             IBusinessUnitService businessUnitService,
                             IUserService userService,
                             IReviewEditor reviewEditor)
        {
            _context = context;
            _businessValidator = businessValidator;
            _businessUnitService = businessUnitService;
            _userService = userService;
            _reviewEditor = reviewEditor;
        }

        public async Task<ReviewDTO> CreateReviewAsync(ReviewModel model)
        {
            //automatic edit 
            var editedDescription = _reviewEditor.AutomaticReviewEditor(model.OriginalDescription);

            //check visibility
            var checkVisibility = _reviewEditor.CheckReviewVisibility(editedDescription);

            var businessUnit = await _businessUnitService.GetBusinessUnitAsync(model.BusinessUnitId);

            var review = new Review()
            {
                OriginalDescription = model.OriginalDescription,
                EditedDescription = editedDescription,
                Rating = model.Rating,
                CreatedOn = DateTime.Now,
                isVisible = checkVisibility,
                BusinessUnitId = businessUnit.Id
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review.ToDTO();
        }

        public async Task<ReviewDTO> UpdateReviewAsync(ReviewModel model)
        {
            var review = await GetReviewAsync(model.Id);

            review.EditedDescription = model.EditedDescription;
            await _context.SaveChangesAsync();

            return review.ToDTO();
        }

        public async Task<ReviewDTO> MakeReviewInvisibleAsync(int reviewId)
        {
            var review = await GetReviewAsync(reviewId);

            review.isVisible = false;
            await _context.SaveChangesAsync();

            return review.ToDTO();
        }

        public async Task<IReadOnlyCollection<ReviewDTO>> GetReviewsByBusinessUnitAsync(int businessUnitId)
        {
            var businessUnit = await _businessUnitService.GetBusinessUnitAsync(businessUnitId);

            var result = await _context.Reviews
                                .Where(bu => bu.BusinessUnitId == businessUnit.Id)
                                .Where(co => co.isVisible)
                                .OrderByDescending(co => co.CreatedOn)
                                .Select(r => r.ToDTO())
                                .ToListAsync();

            return result;
        }

        public async Task<IReadOnlyCollection<ReviewDTO>> GetReviewsByModeratorAsync(string moderatorId)
        {
            var moderator = await _userService.GetUserAsync(moderatorId);

            var result = await this._context.Reviews
                                    .Where(bu => bu.BusinessUnitId == moderator.BusinessUnitId)
                                    .OrderByDescending(co => co.CreatedOn)
                                    .Select(r => r.ToDTO())
                                    .ToListAsync();
            return result;
        }

        private async Task<Review> GetReviewAsync(int reviewId)
        {
            var review = await this._context.Reviews
                                    .SingleOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                throw new NotFoundException(ServicesConstants.ReviewNotFound);
            }

            return review;
        }
    }
}
