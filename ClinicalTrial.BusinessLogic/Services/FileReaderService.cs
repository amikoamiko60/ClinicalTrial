using ClinicalTrial.BusinessLogic.Exceptions;
using ClinicalTrial.BusinessLogic.Helpers.ErrorMessages;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClinicalTrial.BusinessLogic.Services
{
    public sealed class FileReaderService : IFileReaderService
    {
        public async Task<string> ReadFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (file is null)
            {
                throw new BusinessException(BusinessErrorMessages.FileShouldNotBeNull);
            }

            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync(cancellationToken);
        }
    }
}
