using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceAuth;
using PetSocialNetwork.ServiceNotification;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthClient _authClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly INotificationClient _notificationClient;
        private readonly IPersonalPhotoClient _personalPhotoClient;
        public AuthService(IAuthClient authClient,
            IUserProfileClient userProfileClient,
            INotificationClient notificationClient,
            IPersonalPhotoClient personalPhotoClient)
        {
            _authClient = authClient
                ?? throw new ArgumentNullException(nameof(authClient));
            _userProfileClient = userProfileClient
                ?? throw new ArgumentNullException(nameof(userProfileClient));
            _notificationClient = notificationClient
                ?? throw new ArgumentNullException(nameof(notificationClient));
            _personalPhotoClient = personalPhotoClient
                ?? throw new ArgumentNullException(nameof(personalPhotoClient));
        }

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

            await _personalPhotoClient.AddAndSetPersonalPhotoAsync(new AddPersonalPhotoRequest()
            {
                ProfileId = userProfile.Id,
                FileBytes = await file.ReadBytesAsync(cancellationToken),
                OriginalFileName = file.FileName
            }, cancellationToken);

            var message = "Поздравляем с регистрацией в нашей социальной сети!";
            var subject = "Регистрация";

            await _notificationClient.SendEmailAsync(new EmailRequest()
            {
                RecepientEmail = account.Email,
                Message = message,
                Subject = subject
            }, cancellationToken);

            return account;
        }

        public async Task<ActionResult<LoginResponse>> LoginByPassword
            (LoginRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.LoginByPasswordAsync(request, cancellationToken);
        }

        public async Task UpdatePassword
            (UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.UpdatePasswordAsync(request, cancellationToken);
        }

        public async Task ResetPassword
            (ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.ResetPasswordAsync(request, cancellationToken);
        }

        public async Task<bool> IsRegisterUserAsync
            (ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.IsRegisterUserAsync(request, cancellationToken);
        }

        public async Task<bool> IsTheSameUserPasswordAsync
            (ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.IsTheSameUserPasswordAsync(request, cancellationToken);
        }
    }
}
