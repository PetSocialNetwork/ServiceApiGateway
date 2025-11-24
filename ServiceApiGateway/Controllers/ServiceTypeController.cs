using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly ITypeService _typeService;
        public ServiceTypeController(ITypeService typeService)
        {
            _typeService = typeService
                ?? throw new ArgumentException(nameof(typeService));
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
            return await _typeService.GetServiceTypeByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает все типы услуг
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<ServiceTypeResponse>> GetServiceTypesAsync
            (CancellationToken cancellationToken)
        {
            return await _typeService.GetServiceTypesAsync(cancellationToken);
        }
    }
}
