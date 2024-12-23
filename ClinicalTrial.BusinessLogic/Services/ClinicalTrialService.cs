using ClinicalTrial.BusinessLogic.Services.Interfaces;
using ClinicalTrial.DataAccess;
using ClinicalTrial.DataAccess.Repositories;
using ClinicalTrial.DataContracts;
using ClinicalTrial.DataContracts.Requests;
using ClinicalTrial.DataContracts.Responses;
using Microsoft.AspNetCore.Http;

namespace ClinicalTrial.BusinessLogic.Services
{
    public sealed class ClinicalTrialService
        (IClinicalTrialRepository clinicalTrialRepository,
         IUnitOfWork unitOfWork,
         IJsonValidationService jsonValidationService,
         IFileValidationService fileValidationService,
         IFileReaderService fileReaderService) : IClinicalTrialService
    {
        public async Task<GetTrialResponse> GetTrialAsync(int id, CancellationToken cancellationToken)
        => await clinicalTrialRepository.GetTrialAsync(id, cancellationToken);

        public async Task<IEnumerable<GetTrialResponse>> GetTrialsAsync(GetTrialsRequest request, CancellationToken cancellationToken)
        => await clinicalTrialRepository.GetTrialsAsync(request, cancellationToken);

        public async Task UploadTrialAsync(IFormFile file, CancellationToken cancellationToken)
        {
            fileValidationService.ValidateFile(file);

            var jsonData = await fileReaderService.ReadFileAsync(file, cancellationToken);

            var deserilizedData = jsonValidationService.ValidateAndDeserialize(jsonData);

            if (deserilizedData.Status == nameof(ClinicalTrialStatus.Ongoing) && !deserilizedData.EndDate.HasValue)
            {
                deserilizedData.EndDate = deserilizedData.StartDate.AddMonths(1);
            }

            if (deserilizedData.EndDate.HasValue)
            {
                deserilizedData.Duration = (deserilizedData.EndDate.Value - deserilizedData.StartDate).Days;
            }

            await clinicalTrialRepository.AddTrialAsync(deserilizedData, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
