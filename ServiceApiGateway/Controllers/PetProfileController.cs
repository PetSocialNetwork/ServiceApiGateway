using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePet;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetProfileController : ControllerBase
    {
        private readonly IPetProfileClient _petProfileClient;
        public PetProfileController(IPetProfileClient petProfileClient)
        {
            _petProfileClient = petProfileClient ?? throw new ArgumentException(nameof(petProfileClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileWithAccountAlreadyExistsException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<Guid> AddPetProfileAsync([FromBody] AddPetProfileRequest request, CancellationToken cancellationToken)
        {
            return await _petProfileClient.AddPetProfileAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PetProfileResponse> GetPetProfileByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _petProfileClient.GetPetProfileByIdAsync(id, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePetProfileAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _petProfileClient.DeletePetProfileAsync(id, cancellationToken);
        }

        ////[ProducesResponseType(StatusCodes.Status200OK)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileWithAccountAlreadyExistsException))]
        ////[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<IEnumerable<PetProfileResponse>> GetPetProfilesByAccountIdAsync([FromBody] Guid accountId, CancellationToken cancellationToken)
        {
            return await _petProfileClient.GetPetProfilesByAccountIdAsync(accountId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("[action]")]
        public async Task UpdatePetProfileAsync([FromBody] UpdatePetProfileRequest request, CancellationToken cancellationToken)
        {
            await _petProfileClient.UpdatePetProfileAsync(request, cancellationToken);
        }
    }
}
