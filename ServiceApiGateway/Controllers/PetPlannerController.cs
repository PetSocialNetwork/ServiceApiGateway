using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePetPlanner;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetPlannerController : ControllerBase
    {
        private readonly IPlannerClient _plannerClient;

        public PetPlannerController(IPlannerClient plannerClient)
        {
            _plannerClient = plannerClient ?? throw new ArgumentNullException(nameof(plannerClient));
        }

        [HttpDelete("[action]")]
        public async Task DeleteRecordAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _plannerClient.DeleteRecordAsync(id, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<RecordResponse> GetRecordByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _plannerClient.GetRecordByIdAsync(id, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<RecordResponse> AddRecordAsync([FromBody] RecordRequest request, CancellationToken cancellationToken)
        {
            return await _plannerClient.AddRecordAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<RecordResponse> UpdateRecordAsync([FromBody] UpdateRecordRequest request, CancellationToken cancellationToken)
        {
            return await _plannerClient.UpdateRecordAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<ICollection<RecordResponse>> GetAllRecordsByDateAsync([FromBody] RecordByDateRequest request, CancellationToken cancellationToken)
        {
            return await _plannerClient.GetAllRecordsByDateAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<ICollection<RecordResponse>> GetAllRecordsByPeriodAsync([FromBody] RecordByPeriodRequest request, CancellationToken cancellationToken)
        {
            return await _plannerClient.GetAllRecordsByPeriodAsync(request, cancellationToken);
        }
    }
}
