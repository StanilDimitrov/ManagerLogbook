using ManagerLogbook.Services;
using ManagerLogbook.Services.Contracts;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.Providers;
using ManagerLogbook.Web.Services;
using ManagerLogbook.Web.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ManagerLogbook.Web.Extensions
{
    public static class CustomServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBusinessUnitService, BusinessUnitService>();
            services.AddScoped<IBusinessValidator, BusinessValidator>();
            services.AddScoped<IReviewEditor, ReviewEditor>();
            services.AddScoped<ILogbookService, LogbookService>();
            services.AddScoped<IUserServiceWrapper, UserServiceWapper>();
            services.AddScoped<IImageOptimizer, ImageOptimizer>();

            return services;
        }
    }
}
