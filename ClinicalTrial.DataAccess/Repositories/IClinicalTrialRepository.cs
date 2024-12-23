using ClinicalTrial.DataContracts.Requests;
using ClinicalTrial.DataContracts.Responses;

namespace ClinicalTrial.DataAccess.Repositories
{
    public interface IClinicalTrialRepository
    {
        Task AddTrialAsync(UploadTrialRequest request, CancellationToken cancellationToken);

        Task<GetTrialResponse> GetTrialAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<GetTrialResponse>> GetTrialsAsync(GetTrialsRequest request, CancellationToken cancellationToken);
    }
}
