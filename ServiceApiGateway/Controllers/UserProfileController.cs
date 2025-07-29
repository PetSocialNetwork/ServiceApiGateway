using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService _userProfileService;
        public UserProfileController(IUserService userProfileService)
        {
            _userProfileService = userProfileService 
                ?? throw new ArgumentException(nameof(userProfileService));
        }

        /// <summary>
        /// Возвращает профиль пользователя по идентификатору профиля
        /// </summary>
        /// <param name="id">Идентификатор профиля</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [ProfileCompletionFilter]
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<UserProfileBySearchResponse> GetUserProfileByIdAsync
            ([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _userProfileService.GetUserProfileByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает профиль пользователя по идентификатору аккаунта
        /// </summary>
        /// <param name="id">Идентификатор аккаунта</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<UserProfileBySearchResponse> GetUserProfileByAccountIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _userProfileService.GetUserProfileByAccountIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Удаляет профиль пользователя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор профиля</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteUserProfileAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _userProfileService.DeleteUserProfileAsync(id, cancellationToken);
        }

        /// <summary>
        ///  Удаляет профиль пользователя по идентификатору аккаунта
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteUserProfileByAccountIdAsync([FromQuery] Guid accountId, CancellationToken cancellationToken)
        {
            await _userProfileService.DeleteUserProfileByAccountIdAsync(accountId, cancellationToken);
        }

        /// <summary>
        /// Добавляет профиль пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<UserProfileResponse> AddUserProfileAsync([FromBody] AddUserProfileRequest request, CancellationToken cancellationToken)
        {
            return await _userProfileService.AddUserProfileAsync(request, cancellationToken);
        }

        /// <summary>
        /// Обновляет профиль пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="file">Файл</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdateUserProfileAsync(
            [FromForm] UpdateUserProfileRequest request,
            IFormFile? file, CancellationToken cancellationToken)
        {
           await _userProfileService.UpdateUserProfileAsync(request, file, cancellationToken);
        }

        /// <summary>
        /// Возвращает найденные профили по имени
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<UserProfileBySearchResponse>> FindUserProfileByNameAsync([FromBody] FindUserProfileRequest request, CancellationToken cancellationToken)
        {
            return await _userProfileService.FindUserProfileByNameAsync(request, cancellationToken);
        }
    }
}
