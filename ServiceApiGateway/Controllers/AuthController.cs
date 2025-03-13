using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceAuth;
using PetSocialNetwork.ServiceNotification;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using System.Security.Principal;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthClient _authClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly INotificationClient _notificationClient;
        private readonly IPersonalPhotoClient _photoClient;
        public AuthController(IAuthClient authClient, 
            IUserProfileClient userProfileClient,
            INotificationClient notificationClient,
            IPersonalPhotoClient photoClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _photoClient = photoClient ?? throw new ArgumentNullException(nameof(photoClient));
            _notificationClient = notificationClient ?? throw new ArgumentNullException(nameof(notificationClient));
            _userProfileClient = userProfileClient ?? throw new ArgumentNullException(nameof(userProfileClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidOperationException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<RegisterResponse> Register
            ([FromForm] RegisterRequest request,
            IFormFile file,
            CancellationToken cancellationToken)
        {
            var account = await _authClient.RegisterAsync(request, cancellationToken);
            var userProfile = await _userProfileClient.AddUserProfileAsync(new AddUserProfileRequest()
                {
                    AccountId = account.Id
                }, cancellationToken);

            await _notificationClient.SendEmailAsync(new EmailRequest()
            {
                RecepientEmail = account.Email, 
                Message = "Поздравляем вы успешно зарегистрированы!",
                Subject = "Регистрация"
            }, cancellationToken);

            await using var fileStream = file.OpenReadStream();
            var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
            await _photoClient.AddAndSetPersonalPhotoAsync(userProfile.Id, photo, cancellationToken);
            return account;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> LoginByPassword(LoginRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.LoginByPasswordAsync(request, cancellationToken);
        }

        [Authorize]
        [HttpDelete("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAccount(Guid id, CancellationToken cancellationToken)
        {
            await _authClient.DeleteAccountAsync(id, cancellationToken);
        }

        [Authorize]
        [ProfileCompletionFilter]
        [HttpPut("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PasswordNotChangedException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.UpdatePasswordAsync(request, cancellationToken);
        }

        [Authorize]
        [HttpPut("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.ResetPasswordAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<bool> IsRegisterUserAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.IsRegisterUserAsync(request, cancellationToken);
        }
    }
}
