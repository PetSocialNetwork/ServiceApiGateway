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
    }
}
