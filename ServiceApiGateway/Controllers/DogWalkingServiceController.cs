using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogWalkingServiceController : ControllerBase
    {
        private readonly IDogWalkingService  _dogWalkingService;
        public DogWalkingServiceController(IDogWalkingService dogWalkingService)
        {
            _dogWalkingService = dogWalkingService
                ?? throw new ArgumentException(nameof(dogWalkingService));
        }

        /// <summary>
        /// Возвращает тип услуги прогулки с собаками по ее идентификатору основной услуги
        /// </summary>
        /// <param name="serviceId">Идентификатор типа услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<DogWalkingServiceResponse> GetDogWalkingByServiceIdAsync
            ([FromQuery] Guid serviceId, CancellationToken cancellationToken)
        {
            return await _dogWalkingService.GetDogWalkingByServiceIdAsync(serviceId, cancellationToken);
        }

        /// <summary>
        /// Обновляет услугу прогулки с собаками
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdateDogWalkingAsync
           ([FromBody] UpdateDogWalkingRequest request, CancellationToken cancellationToken)
        {
            await _dogWalkingService.UpdateDogWalkingAsync(request, cancellationToken);
        }
    }
}
