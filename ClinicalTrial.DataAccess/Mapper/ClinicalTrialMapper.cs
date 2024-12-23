using ClinicalTrial.DataAccess.Entities;
using ClinicalTrial.DataContracts.Responses;

namespace ClinicalTrial.DataAccess.Mapper
{
    public static class ClinicalTrialMapper
    {
        public static GetTrialResponse MapEntityToResponse(ClinicalTrialEntity entity)
        {
            return new GetTrialResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Status = entity.Status,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Duration = entity.Duration,
                Participants = entity.Participants,
                TrialId = entity.TrialId
            };
        }

        public static IEnumerable<GetTrialResponse> MapEntityToResponse(IEnumerable<ClinicalTrialEntity> entity)
        {
            return entity.Select(MapEntityToResponse);
        }
    }
}
