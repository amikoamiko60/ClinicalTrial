using ClinicalTrial.BusinessLogic.Services.Interfaces;
using ClinicalTrial.DataContracts.Requests;
using ClinicalTrial.DataContracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalTrial.Api.Controllers
{
    [ApiController]
    [Route("api/trials")]
    public class TrialsController(IClinicalTrialService clinicalTrialService) : ControllerBase
    {
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task UploadTrial(IFormFile file, CancellationToken cancellationToken)
        => await clinicalTrialService.UploadTrialAsync(file, cancellationToken);

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTrialResponse), StatusCodes.Status200OK)]
        public async Task<GetTrialResponse> GetTrial([FromRoute]int id, CancellationToken cancellationToken)
        => await clinicalTrialService.GetTrialAsync(id, cancellationToken);

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetTrialResponse>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<GetTrialResponse>> GetTrials([FromQuery] GetTrialsRequest request, CancellationToken cancellationToken)
        => await clinicalTrialService.GetTrialsAsync(request, cancellationToken);
    }
}
