using ManagerLogbook.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManagerLogbook.Data.SeedData
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>().HasData(
                new Town { Id = 1, Name = "Sofia" },
                new Town { Id = 2, Name = "Plovdiv" },
                new Town { Id = 3, Name = "Burgas" },
                new Town { Id = 4, Name = "Varna" }
            );

            modelBuilder.Entity<BusinessUnitCategory>().HasData(
                new BusinessUnitCategory { Id = 1, Name = "Hotels" },
                new BusinessUnitCategory { Id = 2, Name = "Restaurants" }
            );

            modelBuilder.Entity<NoteCategory>().HasData(
                new NoteCategory { Id = 1, Name = "Task" },
                new NoteCategory { Id = 2, Name = "TODO" },
                new NoteCategory { Id = 3, Name = "Event" },
                new NoteCategory { Id = 4, Name = "Maintenance" },
                new NoteCategory { Id = 5, Name = "Supplying issue" }
            );

            modelBuilder.Entity<CensoredWord>().HasData(
                new CensoredWord { Id = 1, Word = "bastard" },
                new CensoredWord { Id = 2, Word = "ass" },
                new CensoredWord { Id = 3, Word = "cock" },
                new CensoredWord { Id = 4, Word = "dick" },
                new CensoredWord { Id = 5, Word = "bull shit" },
                new CensoredWord { Id = 6, Word = "porn" },
                new CensoredWord { Id = 7, Word = "bitch" },
                new CensoredWord { Id = 8, Word = "fuck" },
                new CensoredWord { Id = 9, Word = "Fuck off" },
                new CensoredWord { Id = 10, Word = "mother fucker" },
                new CensoredWord { Id = 11, Word = "pussy" },
                new CensoredWord { Id = 12, Word = "shit" },
                new CensoredWord { Id = 13, Word = "nigga" },
                new CensoredWord { Id = 14, Word = "son of a bitch" },
                new CensoredWord { Id = 15, Word = "scrotum" }
            );

            modelBuilder.Entity<BusinessUnit>().HasData(
                new BusinessUnit { Id = 1, Name = "Grand Hotel Sofia", Address = "bul. Maria Luiza 42", PhoneNumber = "0897654321", Email = "grandhotel@abv.bg", TownId = 1, BusinessUnitCategoryId = 1 },
                new BusinessUnit { Id = 2, Name = "Mariot", Address = "bul. Vasil Levski 42", PhoneNumber = "0897354213", Email = "mariot@abv.bg", TownId = 2, BusinessUnitCategoryId = 1 },
                new BusinessUnit { Id = 3, Name = "Imperial Hotel", Address = "bul. Hristo Botev 32", PhoneNumber = "0897454324", Email = "imperial@mail.bg", TownId = 3, BusinessUnitCategoryId = 1 },
                new BusinessUnit { Id = 4, Name = "Hotel Palermo", Address = "bul. G. Dimitrov 46", PhoneNumber = "0897454324", Email = "0897656361", TownId = 1 , BusinessUnitCategoryId = 1 },
                new BusinessUnit { Id = 5, Name = "Grand Hotel Plovdiv", Address = "bul. Marica 43", PhoneNumber = "0896654621", Email = "grandhotel@abv.bg", TownId = 2, BusinessUnitCategoryId = 1 },
                 new BusinessUnit { Id = 6, Name = "Sweet Sofia", Address = "Student City", PhoneNumber = "0897554325", Email = "sweet@dir.bg", TownId = 1, BusinessUnitCategoryId = 2 }
                );

            modelBuilder.Entity<Logbook>().HasData(
               new Logbook { Id = 1, Name = "Bar and Dinner", BusinessUnitId = 5 },
               new Logbook { Id = 2, Name = "Sweet Valley", BusinessUnitId = 5 },
               new Logbook { Id = 3, Name = "Ambasador", BusinessUnitId = 3 }
               );

            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = "93ad4deb-b9f7-4a98-9585-8b79963aee55", Name = "Admin", NormalizedName = "ADMIN", },
               new IdentityRole { Id = "6b32cc6d-2fc9-4808-a0a6-b3877bf9a381", Name = "Manager", NormalizedName = "MANAGER" },
               new IdentityRole { Id = "d525385f-0b2d-4db4-a874-a2bf1b27ae3f", Name = "Moderator", NormalizedName = "MODERATOR" }
           );

            User appUser = new User
            {
                Id = "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.bg",
                NormalizedEmail = "ADMIN@ADMIN.BG",
                LockoutEnabled = true,
                SecurityStamp = "QSV7IPN3NQOB7US3NWWJQV2BOPWLAWQC"
            };

            var hasher = new PasswordHasher<User>();
            appUser.PasswordHash = hasher.HashPassword(appUser, "123456");

            modelBuilder.Entity<User>().HasData(appUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "93ad4deb-b9f7-4a98-9585-8b79963aee55", UserId = "9c328abd-e9c0-4271-85fb-c7bb7b8adaaf" }
            );
        }
    }
}

