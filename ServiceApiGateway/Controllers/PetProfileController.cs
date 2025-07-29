using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePet;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetProfileController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetProfileController(IPetService petService)
        {
            _petService = petService 
                ?? throw new ArgumentException(nameof(petService));
        }

        /// <summary>
        /// Добавляет профиль пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="file">Файл</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PetProfileResponse> AddPetProfileAsync(
            [FromForm] AddPetProfileRequest request,
            IFormFile file, CancellationToken cancellationToken)
        {
            return await _petService.AddPetProfileAsync(request, file, cancellationToken);
        }

        /// <summary>
        /// Возвращает данные профиля питомца
        /// </summary>
        /// <param name="id">Идентификатор профиля питомца</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PetProfileBySearchResponse> GetPetProfileByIdAsync
            ([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _petService.GetPetProfileByIdAsync(id, cancellationToken); 
        }

        /// <summary>
        /// Обновляет данные профиля пользователя питомца
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="file">Файл</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdatePetProfileAsync(
            [FromForm] UpdatePetProfileRequest request,
            IFormFile? file, CancellationToken cancellationToken)
        {
           await _petService.UpdatePetProfileAsync(request, file, cancellationToken);
        }

        /// <summary>
        /// Удаляет профиль питомца
        /// </summary>
        /// <param name="petId">Идентификатор питомца</param>
        /// <param name="profileId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeletePetProfileAsync
            ([FromQuery] Guid petId, [FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            await _petService.DeletePetProfileAsync(petId, profileId, cancellationToken);   
        }

        /// <summary>
        /// Возвращает все профили питомцев по идентификатору пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<PetProfileBySearchResponse>> GetPetProfilesAsync
            ([FromBody] Guid profileId, CancellationToken cancellationToken)
        {
            return await _petService.GetPetProfilesAsync(profileId, cancellationToken);
        }
    }
}