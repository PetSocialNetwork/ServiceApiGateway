using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePet;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetProfileController : ControllerBase
    {
        private readonly IPetProfileClient _petProfileClient;
        private readonly IPetPhotoClient _petPhotoClient;
        private readonly ICommentClient _commentClient;
        private readonly IMapper _mapper;
        public PetProfileController(IPetProfileClient petProfileClient,
            IPetPhotoClient petPhotoCleint,
            ICommentClient commentClient,
            IMapper mapper)
        {
            _petProfileClient = petProfileClient ?? throw new ArgumentException(nameof(petProfileClient));
            _petPhotoClient = petPhotoCleint ?? throw new ArgumentException(nameof(petPhotoCleint));
            _commentClient = commentClient ?? throw new ArgumentException(nameof(commentClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<PetProfileResponse> AddPetProfileAsync(
            [FromForm] AddPetProfileRequest request,
            IFormFile file, CancellationToken cancellationToken)
        {
            //TODO:Транзакция
            var response = await _petProfileClient.AddPetProfileAsync(request, cancellationToken);
            await using var fileStream = file.OpenReadStream();
            var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
            await _petPhotoClient.AddAndSetPetPhotoAsync(response.Id, request.ProfileId, photo, cancellationToken);

            return response;
        }

        [HttpGet("[action]")]
        public async Task<PetProfileBySearchResponse> GetPetProfileByIdAsync([FromQuery] Guid id,CancellationToken cancellationToken)
        {
            var profile = await _petProfileClient.GetPetProfileByIdAsync(id, cancellationToken);
            var photo = await _petPhotoClient.GetMainPetPhotoAsync(profile.Id, profile.ProfileId, cancellationToken);
            var response = _mapper.Map<PetProfileBySearchResponse>(profile);
            response.PhotoUrl = photo.FilePath;
            return response;
        }

        [HttpPut("[action]")]
        [Consumes("multipart/form-data")]
        public async Task UpdatePetProfileAsync(
            [FromForm] UpdatePetProfileRequest request,
            IFormFile? file, CancellationToken cancellationToken)
        {
            //TODO:Транзакция
            await _petProfileClient.UpdatePetProfileAsync(request, cancellationToken);
            if (file != null)
            {
                await using var fileStream = file.OpenReadStream();
                var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
                await _petPhotoClient.AddAndSetPetPhotoAsync(request.Id, request.ProfileId, photo, cancellationToken);
            }
        }

        [HttpDelete("[action]")]
        public async Task DeletePetProfileAsync([FromQuery] Guid petId, [FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            //Транзакция
            await _petProfileClient.DeletePetProfileAsync(petId, profileId, cancellationToken);
            var photos = await _petPhotoClient.BySearchAsync(petId, profileId, cancellationToken);
            Guid[] photoIds = photos.Select(p => p.Id).ToArray();
            await Task.WhenAll(
           _commentClient.DeleteAllCommentAsync(photoIds, cancellationToken),
           _petPhotoClient.DeleteAllPetPhotosAsync(petId, profileId, cancellationToken));
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<PetProfileBySearchResponse>> GetPetProfilesAsync([FromBody] Guid profileId, CancellationToken cancellationToken)
        {
            //Транзакция
            var petProfiles = await _petProfileClient.GetPetProfilesAsync(profileId, cancellationToken);
            List<PetProfileBySearchResponse> result = [];

            if (petProfiles != null)
            {
                foreach (var petProfile in petProfiles)
                {
                    var photo = await _petPhotoClient.GetMainPetPhotoAsync(petProfile.Id, profileId, cancellationToken);
                    var response = _mapper.Map<PetProfileBySearchResponse>(petProfile);
                    response.PhotoUrl = photo.FilePath;
                    result.Add(response);
                }
            }
            return result;
        }
    }
}