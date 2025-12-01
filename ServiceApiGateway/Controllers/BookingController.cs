using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceBooking;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService
                ?? throw new ArgumentException(nameof(bookingService));
        }

        /// <summary>
        /// Возвращает все свободные даты по идентификатору услуги
        /// </summary>
        /// <param name="serviceId">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<SlotReponse>> GetAvailableSlotsAsync
            ([FromQuery] Guid serviceId, CancellationToken cancellationToken)
        {
            return await _bookingService
                .GetAvailableSlotsAsync(serviceId, cancellationToken);
        }

        /// <summary>
        /// Добавляет бронирование
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task AddBookingAsync
            ([FromBody] AddBookingRequest request, CancellationToken cancellationToken)
        {
            await _bookingService.AddBookingAsync(request, cancellationToken);
        }

        /// <summary>
        /// Добавляет свобдные слоьы, удаляет ненужные 
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task UpdateSlotsAsync
            ([FromBody] List<AddSlotRequest> request, CancellationToken cancellationToken)
        {
            await _bookingService.UpdateSlotsAsync(request, cancellationToken);
        }
    }
}
