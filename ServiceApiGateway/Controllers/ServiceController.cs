using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    //[ProfileCompletionFilter]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IPetCareService _petCareService;
        public ServiceController(IPetCareService petCareService)
        {
            _petCareService = petCareService
                ?? throw new ArgumentException(nameof(petCareService));
        }

        /// <summary>
        /// Добавляет услугу для пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ServiceResponse> AddServiceAsync
            ([FromBody] AddServiceRequest request, CancellationToken cancellationToken)
        {
            return await _petCareService.AddServiceAsync(request, cancellationToken);
        }

        /// <summary>
        /// Удаляет услугу по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteServiceAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            var result = await _petCareService.DeleteServiceAsync(id, cancellationToken);
            if (!result)
            {
                return BadRequest("Невозможно удалить услугу");
            }

            return Ok();
        }

        /// <summary>
        /// Возвращает услуги по идентификатору 
        /// </summary>
        /// <param name="serviceId">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ServiceResponse> GetServiceByIdAsync
            ([FromQuery] Guid serviceId, CancellationToken cancellationToken)
        {
            return await _petCareService.GetServiceByIdAsync(serviceId, cancellationToken);
        }

        /// <summary>
        /// Обновляет услугу
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdateServiceAsync([FromBody] UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            await _petCareService.UpdateServiceAsync(request, cancellationToken);
        }

        /// <summary>
        /// Возвращает все услуги по идентификатору пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор профиля пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<ServiceWithPhotoResponse>> GetServiceByProfileIdAsync
            ([FromBody] Guid profileId, CancellationToken cancellationToken)
        {
            return await _petCareService.GetServiceByProfileIdAsync(profileId, cancellationToken);
        }
    }
}
