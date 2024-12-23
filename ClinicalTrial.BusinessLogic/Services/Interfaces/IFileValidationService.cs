using Microsoft.AspNetCore.Http;

namespace ClinicalTrial.BusinessLogic.Services.Interfaces
{
    public interface IFileValidationService
    {
        void ValidateFile(IFormFile file);
    }
}
