using ClinicalTrial.DataContracts.Requests;

namespace ClinicalTrial.BusinessLogic.Services.Interfaces
{
    public interface IJsonValidationService
    {
        UploadTrialRequest ValidateAndDeserialize(string jsonData);
    }
}
