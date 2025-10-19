using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
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
        /// Возвращает услугу по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ServiceResponse> GetServiceByIdAsync
            ([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _petCareService.GetServiceByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Удаляет услушу по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteServiceAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _petCareService.DeleteServiceAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает все услуги по идентификатору пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор профиля пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<ServiceResponse>> GetServiceByProfileIdAsync
            ([FromBody] Guid profileId, CancellationToken cancellationToken)
        {
            return await _petCareService.GetServiceByProfileIdAsync(profileId, cancellationToken);
        }

        /// <summary>
        /// Возвращает тип услуги по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор типа услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ServiceTypeResponse> GetServiceTypeByIdAsync
            ([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _petCareService.GetServiceTypeByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает все типы услуг
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<ServiceTypeResponse>> GetServiceTypesAsync
            (CancellationToken cancellationToken)
        {
            return await _petCareService.GetServiceTypesAsync(cancellationToken);
        }
    }
}
