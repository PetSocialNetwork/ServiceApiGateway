using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPersonalPhotoClient _personalPhotoClient;
        private readonly IMapper _mapper;
        public UserProfileController(IUserProfileClient userProfileClient,
            IPersonalPhotoClient personalPhotoClient,
            IMapper mapper)
        {
            _userProfileClient = userProfileClient ?? throw new ArgumentException(nameof(userProfileClient));
            _personalPhotoClient = personalPhotoClient ?? throw new ArgumentException(nameof(personalPhotoClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[ProfileCompletionFilter]
        [HttpGet("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserProfileBySearchResponse> GetUserProfileByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            //Транзакция
            var userProfile = await _userProfileClient.GetUserProfileByIdAsync(id, cancellationToken);
            var photo = await _personalPhotoClient.GetMainPersonalPhotoAsync(userProfile.Id, cancellationToken);
            var response = _mapper.Map<UserProfileBySearchResponse>(userProfile);
            response.PhotoUrl = photo.FilePath;
            return response;
        }

        [HttpGet("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserProfileResponse> GetUserProfileByAccountIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return  await _userProfileClient.GetUserProfileByAccountIdAsync(id, cancellationToken);
        }

        [HttpDelete("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteUserProfileAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _userProfileClient.DeleteUserProfileAsync(id, cancellationToken);
        }

        [HttpDelete("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteUserProfileByAccountIdAsync([FromQuery] Guid accountId, CancellationToken cancellationToken)
        {
            await _userProfileClient.DeleteUserProfileByAccountIdAsync(accountId, cancellationToken);
        }

        [HttpPost("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileWithAccountAlreadyExistsException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserProfileResponse> AddUserProfileAsync([FromBody] AddUserProfileRequest request, CancellationToken cancellationToken)
        {
            return await _userProfileClient.AddUserProfileAsync(request, cancellationToken);
        }

        [HttpPut("[action]")]
        [Consumes("multipart/form-data")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateUserProfileAsync(
            [FromForm] UpdateUserProfileRequest request,
            IFormFile? file, CancellationToken cancellationToken)
        {
            //Транзакция
            await _userProfileClient.UpdateUserProfileAsync(request, cancellationToken);
            if (file != null)
            {
                await using var fileStream = file.OpenReadStream();
                var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
                await _personalPhotoClient.AddAndSetPersonalPhotoAsync(request.Id, photo, cancellationToken);
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserProfileBySearchResponse>> FindUserProfileByNameAsync([FromQuery] string firstName, [FromQuery] string lastName, CancellationToken cancellationToken)
        {
            var userProfiles = await _userProfileClient.FindUserProfileByNameAsync(firstName, lastName, cancellationToken);
            var userProfileIds = userProfiles.Select(x => x.Id).ToList();
            var photos = await _personalPhotoClient.GetMainPersonalPhotoByIdsAsync(userProfileIds, cancellationToken);
            var photoDictionary = photos.ToDictionary(p => p.ProfileId, p => p.FilePath);

            var result = userProfiles.Select(userProfile =>
            {
                var photoUrl = photoDictionary.GetValueOrDefault(userProfile.Id);

                return new UserProfileBySearchResponse
                {
                    Id = userProfile.Id,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    DateOfBirth = userProfile.DateOfBirth,
                    WalksDogs = userProfile.WalksDogs,
                    Profession = userProfile.Profession,
                    AccountId = userProfile.AccountId,
                    IsProfileCompleted = userProfile.IsProfileCompleted,
                    PhotoUrl = photoUrl
                };
            }).ToList();

            return result;
        }
    }
}
