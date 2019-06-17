using ManagerLogbook.Services.DTOs;
using ManagerLogbook.Web.Models;
using System;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public class TestHelpersReviewController
    {
        public static ReviewViewModel TestReviewViewModel01()
        {
            return new ReviewViewModel
            {
                OriginalDescription = "This is first review",
                BusinessUnitId = 1,
                Rating = 1
            };
        }

        public static ReviewDTO TestReviewDTO01()
        {
            return new ReviewDTO
            {
                OriginalDescription = "This is first review",
                BusinessUnitId = 1,
                Rating = 1
            };
        }

        public static ReviewDTO TestReviewDTO02()
        {
            return new ReviewDTO
            {
                Id=2,
                OriginalDescription = "This is second review",
                EditedDescription = "This is EDIT second review",
                BusinessUnitId = 2,
                Rating = 1
            };
        }

        public static ReviewDTO TestReviewDTO03()
        {
            return new ReviewDTO
            {
                Id = 3,
                OriginalDescription = "This is third review",
                EditedDescription = "This is EDIT third review",
                isVisible = false,
                BusinessUnitId = 3,
                Rating = 1
            };
        }
    }
}
