using PetSocialNetwork.ServiceBooking;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ICollection<SlotReponse>> GetAvailableSlotsAsync
            (Guid serviceId, CancellationToken cancellationToken);
        Task AddBookingAsync
            (AddBookingRequest request, CancellationToken cancellationToken);
        Task UpdateSlotsAsync
            (Guid serviceId, List<AddSlotRequest> request, CancellationToken cancellationToken);
        Task<IEnumerable<BookingResponse>> GetBookingsByServiceIdAsync
            (Guid serviceId, CancellationToken cancellationToken);
        Task<IEnumerable<BookingResponse>> GetBookingsProfileIdAsync
            (Guid profileId, CancellationToken cancellationToken);
        Task UpdateBookingStatusAsync
           (UpdateBookingStatusRequest request, CancellationToken cancellationToken);
        Task DeleteBookingAsync
           (Guid bookingId, CancellationToken cancellationToken);
    }
}
