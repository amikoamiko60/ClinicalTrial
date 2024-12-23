using ClinicalTrial.DataContracts.Requests;
using ClinicalTrial.DataContracts.Responses;
using Microsoft.AspNetCore.Http;

namespace ClinicalTrial.BusinessLogic.Services.Interfaces
{
    public interface IClinicalTrialService
    {
        Task<GetTrialResponse> GetTrialAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<GetTrialResponse>> GetTrialsAsync(GetTrialsRequest request, CancellationToken cancellationToken);

        Task UploadTrialAsync(IFormFile file, CancellationToken cancellationToken);
    }
}
