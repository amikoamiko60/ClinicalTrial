using Microsoft.AspNetCore.Http;

namespace ClinicalTrial.BusinessLogic.Services.Interfaces
{
    public interface IFileReaderService
    {
        Task<string> ReadFileAsync(IFormFile file, CancellationToken cancellationToken);
    }
}
