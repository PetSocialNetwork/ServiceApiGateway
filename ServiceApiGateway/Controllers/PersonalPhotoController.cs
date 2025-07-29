using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPhotoController : ControllerBase
    {
        private readonly IPersonalPhotoService _personalPhotoService;
        public PersonalPhotoController(
            IPersonalPhotoService personalPhotoService)
        {
            _personalPhotoService = personalPhotoService 
                ?? throw new ArgumentException(nameof(personalPhotoService));
        }

        /// <summary>
        /// Добавляет фотографию пользователю
        /// </summary>
        /// <param name="profileId">Идентификатор пользователя</param>
        /// <param name="file">Файл</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse> AddPersonalPhotoAsync
            ([FromForm] Guid profileId,
            IFormFile file, CancellationToken cancellationToken)
        {
            return await _personalPhotoService.AddPersonalPhotoAsync(profileId, file, cancellationToken);
        }

        /// <summary>
        /// Удаляет фотографию пользователя по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeletePersonalPhotoAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            await _personalPhotoService.DeletePersonalPhotoAsync(photoId, cancellationToken);
        }

        /// <summary>
        /// Возвращает фотографию пользователя по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            return await _personalPhotoService.GetPersonalPhotoByIdAsync(photoId, cancellationToken);
        }

        /// <summary>
        /// Возвращает главную фотографию пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор профиля</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse?> GetMainPersonalPhotoAsync([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            return await _personalPhotoService.GetMainPersonalPhotoAsync(profileId, cancellationToken);
        }

        /// <summary>
        /// Возвращает все фотографии пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<PersonalPhotoResponse>> BySearchAsync([FromBody] PersonalPhotoBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoService.BySearchAsync(request, cancellationToken);
        }

        /// <summary>
        /// Устанавливает главную фотографию пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync([FromBody] PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoService.SetMainPersonalPhotoAsync(request, cancellationToken);
        }

        /// <summary>
        /// Удаляет все фотографии пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор профиля</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task DeleteAllPersonalPhotosAsync([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            await _personalPhotoService.DeleteAllPersonalPhotosAsync(profileId, cancellationToken);
        }
    }
}
