using ClinicalTrial.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicalTrial.DataAccess
{
    public static class DataAccessRegistrar
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ClinicalTrial")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClinicalTrialRepository, ClinicalTrialRepository>();
            return services;
        }
    }
}
