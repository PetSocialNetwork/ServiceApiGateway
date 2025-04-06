using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePetPlanner;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetPlannerController : ControllerBase
    {
        private readonly IPetPlannerClient _petPlannerClient;

        public PetPlannerController(IPetPlannerClient petPlannerClient,
            IMapper mapper)
        {
            _petPlannerClient = petPlannerClient ?? throw new ArgumentNullException(nameof(petPlannerClient));
        }

        [HttpDelete("[action]")]
        public async Task DeleteRecordAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _petPlannerClient.DeleteRecordAsync(id, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<RecordResponse> GetRecordByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _petPlannerClient.GetRecordByIdAsync(id, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<RecordResponse> AddRecordAsync([FromBody] RecordRequest request, CancellationToken cancellationToken)
        {
            return await _petPlannerClient.AddRecordAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<RecordResponse> UpdateRecordAsync([FromBody] UpdateRecordRequest request, CancellationToken cancellationToken)
        {
            return await _petPlannerClient.UpdateRecordAsync(request, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<ICollection<RecordResponse>> GetAllRecordsAsync(CancellationToken cancellationToken)
        {
            return await _petPlannerClient.GetAllRecordsAsync(cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<ICollection<RecordResponse>> GetAllRecordsByDateAsync([FromBody] RecordByDateRequest request, CancellationToken cancellationToken)
        {
            return await _petPlannerClient.GetAllRecordsByDateAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<ICollection<RecordResponse>> GetAllRecordsByPeriodAsync([FromBody] RecordByPeriodRequest request, CancellationToken cancellationToken)
        {
            return await _petPlannerClient.GetAllRecordsByPeriodAsync(request, cancellationToken);
        }
    }
}
