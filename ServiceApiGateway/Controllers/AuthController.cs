using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceAuth;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServiceNotification;
using PetSocialNetwork.ServicePet;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthClient _authClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPetProfileClient _petProfileClient;
        private readonly INotificationClient _notificationClient;
        private readonly IPetPhotoClient _petPhotoClient;
        private readonly ICommentClient _commentClient;
        private readonly IPersonalPhotoClient _photoClient;
        public AuthController(IAuthClient authClient, 
            IUserProfileClient userProfileClient,
            IPetProfileClient petProfileClient,
            INotificationClient notificationClient,
            IPersonalPhotoClient photoClient,
            IPetPhotoClient petPhotoClient,
            ICommentClient commentClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _photoClient = photoClient ?? throw new ArgumentNullException(nameof(photoClient));
            _petPhotoClient = petPhotoClient ?? throw new ArgumentNullException(nameof(petPhotoClient));
            _notificationClient = notificationClient ?? throw new ArgumentNullException(nameof(notificationClient));
            _userProfileClient = userProfileClient ?? throw new ArgumentNullException(nameof(userProfileClient));
            _petProfileClient = petProfileClient ?? throw new ArgumentNullException(nameof(petProfileClient));
            _commentClient = commentClient ?? throw new ArgumentNullException(nameof(commentClient));
        }

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
                Message = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Успешная регистрация!</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                line-height: 1.6;
                                color: #333;
                                background-color: #f4f4f4;
                                padding: 20px;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #fff;
                                padding: 30px;
                                border-radius: 5px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h1 {{
                                color: #007bff;
                                margin-bottom: 20px;
                            }}
                            p {{
                                margin-bottom: 15px;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                background-color: #007</h1>
                            <p>Вы успешно зарегистрировались на нашем сайте. Теперь вы можете воспользоваться всеми преимуществами нашей платформы.</p>
                            <p>Спасибо за регистрацию!</p>
                            <a href=""#"" class=""button"">Начать пользоваться</a>
                        </div>
                    </body>
                    </html>
                ",
                Subject = "Регистрация"
            }, cancellationToken);

            await using var fileStream = file.OpenReadStream();
            var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
            await _photoClient.AddAndSetPersonalPhotoAsync(userProfile.Id, photo, cancellationToken);
            return account;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> LoginByPassword(LoginRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.LoginByPasswordAsync(request, cancellationToken);
        }

        [Authorize]
        [HttpDelete("[action]")]
        public async Task DeleteAccount([FromQuery] Guid accountId, [FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            var pets = await _petProfileClient.GetPetProfilesAsync(profileId, cancellationToken);
            var photos = new List<PetPhotoReponse>();
            foreach (var pet in pets)
            {
                var petPhotos = await _petPhotoClient.BySearchAsync(pet.Id, profileId, cancellationToken);
                var mainPetPhoto = await _petPhotoClient.GetMainPetPhotoAsync(pet.Id, profileId, cancellationToken);
                photos.AddRange(petPhotos);
                await _petPhotoClient.DeleteAllPetPhotosAsync(pet.Id, profileId, cancellationToken);
                await _petPhotoClient.DeletePetPhotoAsync(mainPetPhoto.Id, cancellationToken);
            }

            var photoIds = photos.Select(p => p.Id).ToList();
            await _commentClient.DeleteAllCommentAsync(photoIds, cancellationToken);
            await _petProfileClient.DeleteAllPetProfilesAsync(profileId, cancellationToken);


            var personalPhotos = await _photoClient.BySearchAsync(profileId, cancellationToken);
            var personalPhotoIds = personalPhotos.Select(p => p.Id).ToList();
            await _commentClient.DeleteAllCommentAsync(personalPhotoIds, cancellationToken);
            await _photoClient.DeleteAllPersonalPhotosAsync(profileId, cancellationToken);
        }

        [Authorize]
        [ProfileCompletionFilter]
        [HttpPut("[action]")]
        public async Task UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.UpdatePasswordAsync(request, cancellationToken);
        }

        [HttpPut("[action]")]
        public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.ResetPasswordAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<bool> IsRegisterUserAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.IsRegisterUserAsync(request, cancellationToken);
        }
    }
}
