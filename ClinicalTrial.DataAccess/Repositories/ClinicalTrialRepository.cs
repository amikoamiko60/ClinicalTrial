using ClinicalTrial.DataAccess.Entities;
using ClinicalTrial.DataAccess.Mapper;
using ClinicalTrial.DataContracts;
using ClinicalTrial.DataContracts.Requests;
using ClinicalTrial.DataContracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrial.DataAccess.Repositories
{
    public sealed class ClinicalTrialRepository(ApplicationDbContext db) : IClinicalTrialRepository
    {
        public async Task AddTrialAsync(UploadTrialRequest request, CancellationToken cancellationToken)
        {
            var entity = new ClinicalTrialEntity
            {
                TrialId = request.TrialId,
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Participants = request.Participants,
                Status = Enum.Parse<ClinicalTrialStatus>(request.Status, true),
                Duration = request.Duration
            };

            await db.AddAsync(entity, cancellationToken);
        }

        public async Task<GetTrialResponse> GetTrialAsync(int id, CancellationToken cancellationToken)
        {
            var query = GetTrialsQuery(new GetTrialsRequest() { Id = id });

            var trial = await query.FirstOrDefaultAsync(cancellationToken);

            if (trial == null)
            {
                return null;
            }

            return ClinicalTrialMapper.MapEntityToResponse(trial);
        }

        public async Task<IEnumerable<GetTrialResponse>> GetTrialsAsync(GetTrialsRequest request, CancellationToken cancellationToken)
        {
            var query = GetTrialsQuery(request);

            var trials = await query.ToListAsync(cancellationToken);

            return trials?.Any() != true ? Enumerable.Empty<GetTrialResponse>() : ClinicalTrialMapper.MapEntityToResponse(trials);
        }

        private IQueryable<ClinicalTrialEntity> GetTrialsQuery(GetTrialsRequest request)
        {
            var query = db.ClinicalTrials.AsQueryable();

            if (request != null)
            {
                if (request.Id.HasValue)
                {
                    query = query.Where(a => a.Id == request.Id.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    if (Enum.TryParse<ClinicalTrialStatus>(request.Status, true, out var statusEnum))
                    {
                        query = query.Where(a => a.Status == statusEnum);
                    }
                }

                if (request.StartDate.HasValue)
                {
                    query = query.Where(a => a.StartDate >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    query = query.Where(a => a.EndDate <= request.EndDate.Value);
                }
            }

            return query;
        }
    }
}
