using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Tests.HelpersMethods
{
    public class TestHelperReview
    {
        public static Review Review01()
        {
            return new Review
            {
                Id = 1,
                OriginalDescription = "Original Text of Review01",
                EditedDescription = "Original Text of Review01",
                Rating = 1,
                CreatedOn = new DateTime(01 / 07 / 2004),
                isVisible = true,
                BusinessUnitId = 1
            };
        }

        public static Review Review02()
        {
            return new Review
            {
                Id = 2,
                OriginalDescription = "Original Text of Review02",
                EditedDescription = "Edited Text of Review02",
                Rating = 5,
                CreatedOn = new DateTime(04 / 14 / 2013),
                isVisible = true,
                BusinessUnitId = 1
            };
        }

        public static Review Review03()
        {
            return new Review
            {
                Id = 3,
                OriginalDescription = "Original Text of Review03",
                EditedDescription = "Edited Text of Review03",
                Rating = 5,
                CreatedOn = new DateTime(04 / 14 / 2013),
                isVisible = false,
                BusinessUnitId = 1
            };
        }

        public static Review Review04()
        {
            return new Review
            {
                Id = 4,
                OriginalDescription = "Original Text of Review04",
                EditedDescription = "Edited Text of Review04",
                Rating = 5,
                CreatedOn = new DateTime(04 / 14 / 2020),
                isVisible = false,
                BusinessUnitId = 1
            };
        }

        public static BusinessUnit TestBusinessUnit01()
        {
            return new BusinessUnit
            {
                Id = 1,
                Name = "Kempinski",
                Address = "Cherni Vryh 15",
                PhoneNumber = "1111111111",
                Email = "info@kempinski.com",
                BusinessUnitCategoryId = 1
            };
        }

        public static User TestUser01()
        {
            return new User
            {
                Id = "70c946ce-0c88-4084-ab2e-dfbf53783c05",
                UserName = "ivan",
                Email = "dir@bg",
                PasswordHash = "123456",
                BusinessUnitId =1
            };
        }
    }
}
