using AutoMapper;
using PetSocialNetwork.ServiceBooking;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingClient _bookingClient;
        public BookingService(IBookingClient bookingClient,
            IMapper mapper)
        {
            _bookingClient = bookingClient
                ?? throw new ArgumentException(nameof(bookingClient));
        }
        public async Task<ICollection<SlotReponse>> GetAvailableSlotsAsync
            (Guid serviceId, CancellationToken cancellationToken)
        {
            return await _bookingClient
                .GetAvailableSlotsAsync(serviceId, cancellationToken);
        }

        public async Task AddBookingAsync
            (AddBookingRequest request, CancellationToken cancellationToken)
        {
            await _bookingClient.AddBookingAsync(request, cancellationToken);
        }

        public async Task UpdateSlotsAsync
            (Guid serviceId, List<AddSlotRequest> request, CancellationToken cancellationToken)
        {
            await _bookingClient.UpdateSlotsAsync(serviceId, request, cancellationToken);
        }
    }
}
