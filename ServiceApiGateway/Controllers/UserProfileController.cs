using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceUser;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileClient _userProfileClient;
        public UserProfileController(IUserProfileClient userProfileClient)
        {
            _userProfileClient = userProfileClient ?? throw new ArgumentException(nameof(userProfileClient));
        }

        [ProfileCompletionFilter]
        [HttpGet("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserProfileResponse> GetUserProfileByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _userProfileClient.GetUserProfileByIdAsync(id, cancellationToken);
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
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserProfileNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task UpdateUserProfileAsync([FromBody] UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            await _userProfileClient.UpdateUserProfileAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserProfileResponse>> FindUserProfileByNameAsync([FromQuery] string firstName, [FromQuery] string lastName, CancellationToken cancellationToken)
        {
            return await _userProfileClient.FindUserProfileByNameAsync(firstName, lastName, cancellationToken);   
        }
    }
}
