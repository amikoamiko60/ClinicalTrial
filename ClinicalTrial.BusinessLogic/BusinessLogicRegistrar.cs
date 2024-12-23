using ClinicalTrial.BusinessLogic.Services;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicalTrial.BusinessLogic
{
    public static class BusinessLogicRegistrar
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IClinicalTrialService, ClinicalTrialService>();
            services.AddScoped<IJsonValidationService, JsonValidationService>();
            services.AddScoped<IFileValidationService, FileValidationService>();
            services.AddScoped<IFileReaderService, FileReaderService>();
            services.AddScoped<IEmbeddedResourceProvider, EmbeddedResourceProvider>();
            return services;
        }
    }
}
