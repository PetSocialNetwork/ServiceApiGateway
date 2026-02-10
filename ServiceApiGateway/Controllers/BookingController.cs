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
        /// Возвращает все бронирования по идентификатору услуги
        /// </summary>
        /// <param name="serviceId">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<BookingResponse>> GetBookingsByServiceIdAsync
            ([FromQuery] Guid serviceId, CancellationToken cancellationToken)
        {
            return await _bookingService
                .GetBookingsByServiceIdAsync(serviceId, cancellationToken);
        }

        /// <summary>
        /// Возвращает все бронирования по идентификатору пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<BookingResponse>> GetBookingsByProfileIdAsync
            ([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            return await _bookingService
                .GetBookingsProfileIdAsync(profileId, cancellationToken);
        }

        /// <summary>
        /// Обновляет статус бронирования
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task UpdateBookingStatusAsync
            ([FromBody] UpdateBookingStatusRequest request, CancellationToken cancellationToken)
        {
            await _bookingService.UpdateBookingStatusAsync
                (request, cancellationToken);
        }

        /// <summary>
        /// Обновляет статус бронирования
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task DeleteBookingAsync
            ([FromQuery] Guid bookingId, CancellationToken cancellationToken)
        {
            await _bookingService.DeleteBookingAsync
                (bookingId, cancellationToken);
        }

        /// <summary>
        /// Добавляет свобдные слоты, удаляет ненужные 
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task UpdateSlotsAsync
          ( [FromQuery] Guid serviceId,
            [FromBody] List<AddSlotRequest> request,
            CancellationToken cancellationToken)
        {
            await _bookingService.UpdateSlotsAsync(serviceId, request, cancellationToken);
        }
    }
}
