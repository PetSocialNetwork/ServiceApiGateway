using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceAuth;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService
                ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        ///  Регистрация
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="file">Файл</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<RegisterResponse> Register
           ([FromForm] RegisterRequest request,
            IFormFile file,
            CancellationToken cancellationToken)
        {
            return await _authService.Register(request, file, cancellationToken);
        }

        /// <summary>
        /// Вход по паролю
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> LoginByPassword(LoginRequest request, CancellationToken cancellationToken)
        {
            return await _authService.LoginByPassword(request, cancellationToken);
        }

        /// <summary>
        /// Обновляет пароль
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [Authorize]
        [ProfileCompletionFilter]
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.UpdatePassword(request, cancellationToken);
        }

        /// <summary>
        /// Сбрасывает пароль
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.ResetPassword(request, cancellationToken);
        }

        /// <summary>
        /// Проверяет, зарегистрирован ли пользователь
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отменыы</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<bool> IsRegisterUserAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authService.IsRegisterUserAsync(request, cancellationToken);
        }

        /// <summary>
        /// Проверяет, является ли пароль тем же самым
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<bool> IsTheSameUserPasswordAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authService.IsTheSameUserPasswordAsync(request, cancellationToken);
        }
    }
}
