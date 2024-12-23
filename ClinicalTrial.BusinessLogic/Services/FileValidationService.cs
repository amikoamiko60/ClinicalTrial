using ClinicalTrial.BusinessLogic.Exceptions;
using ClinicalTrial.BusinessLogic.Helpers.ErrorMessages;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClinicalTrial.BusinessLogic.Services
{
    public sealed class FileValidationService : IFileValidationService
    {
        public void ValidateFile(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName)?.ToLower() != ".json")
            {
                throw new BusinessException(BusinessErrorMessages.InvalidFileType);
            }

            if (file.Length > 1 * 1024 * 1024)
            {
                throw new BusinessException(BusinessErrorMessages.FileSizeExceeds);
            }
        }
    }
}
