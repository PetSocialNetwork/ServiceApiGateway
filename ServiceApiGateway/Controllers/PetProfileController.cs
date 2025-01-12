using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePet;
using PetSocialNetwork.ServicePhoto;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetProfileController : ControllerBase
    {
        private readonly IPetProfileClient _petProfileClient;
        private readonly IPetPhotoClient _petPhotoCleint;
        public PetProfileController(IPetProfileClient petProfileClient, IPetPhotoClient petPhotoCleint)
        {
            _petProfileClient = petProfileClient ?? throw new ArgumentException(nameof(petProfileClient));
            _petPhotoCleint = petPhotoCleint ?? throw new ArgumentException(nameof(petPhotoCleint));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileWithAccountAlreadyExistsException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<PetProfileResponse> AddPetProfileAsync(
            [FromForm] AddPetProfileRequest request,
            IFormFile file, CancellationToken cancellationToken)
        {
            //TODO:Транзакция
            var response = await _petProfileClient.AddPetProfileAsync(request, cancellationToken);
            await using var fileStream = file.OpenReadStream();
            var p = new FileParameter(fileStream, file.ContentType);
            await _petPhotoCleint.AddAndSetPetPhotoAsync(p, response.Id, request.AccountId, cancellationToken);

            return response;
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
        public async Task DeletePetProfileAsync([FromQuery] Guid petId, [FromQuery] Guid accountId, CancellationToken cancellationToken)
        {
            //TODO: Транзакция
            await _petProfileClient.DeletePetProfileAsync(petId, cancellationToken);
            await _petPhotoCleint.DeleteAllPetPhotosAsync(petId, accountId, cancellationToken);
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
        [Consumes("multipart/form-data")]
        public async Task UpdatePetProfileAsync(
            [FromForm] UpdatePetProfileRequest request,
            IFormFile file, CancellationToken cancellationToken)
        {
            //TODO:Транзакция
            await _petProfileClient.UpdatePetProfileAsync(request, cancellationToken);
            await using var fileStream = file.OpenReadStream();
            var p = new FileParameter(fileStream, file.ContentType);
            await _petPhotoCleint.AddAndSetPetPhotoAsync(p, request.Id, request.AccountId, cancellationToken);
        }
    }
}
