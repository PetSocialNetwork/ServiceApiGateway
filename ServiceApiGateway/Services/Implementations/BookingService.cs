using PetSocialNetwork.ServiceAuth;
using PetSocialNetwork.ServiceBooking;
using PetSocialNetwork.ServiceNotification;
using PetSocialNetwork.ServicePetCare;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingClient _bookingClient;
        private readonly INotificationClient _notificationClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly IAuthClient _authClient;
        private readonly IServiceClient _serviceClient;
        public BookingService(
            IBookingClient bookingClient,
            INotificationClient notificationClient,
            IUserProfileClient userProfileClient,
            IAuthClient authClient,
            IServiceClient serviceClient)
        {
            _bookingClient = bookingClient
                ?? throw new ArgumentException(nameof(bookingClient));
            _notificationClient = notificationClient
                ?? throw new ArgumentException(nameof(notificationClient));
            _userProfileClient = userProfileClient
                ?? throw new ArgumentException(nameof(userProfileClient));
            _authClient = authClient
                ?? throw new ArgumentException(nameof(authClient));
            _serviceClient = serviceClient
                ?? throw new ArgumentException(nameof(serviceClient));
        }
        public async Task<ICollection<SlotReponse>> GetAvailableSlotsAsync
            (Guid serviceId, CancellationToken cancellationToken)
        {
            return await _bookingClient
                .GetAvailableSlotsAsync(serviceId, cancellationToken);
        }

        public async Task AddBookingAsync(AddBookingRequest request, CancellationToken cancellationToken)
        {
            var client = await _userProfileClient.GetUserProfileByIdAsync(request.ProfileId, cancellationToken);
            var clientAccount = await _authClient.GetAccountByIdAsync(client.AccountId, cancellationToken);
            var message = "Бронирование добавлено. Посмотреть дополнительную информацию вы можете на сайте.";
            var subject = "Бронирование услуги.";
            var clientEmail = new EmailRequest()
            {
                RecepientEmail = clientAccount.Email,
                Message = message,
                Subject = subject
            };

            var service = await _serviceClient.GetServiceByIdAsync(request.ServiceId, cancellationToken);
            var serviceProvider = await _userProfileClient.GetUserProfileByIdAsync(service.ProfileId, cancellationToken);
            var serviceProviderAccount = await _authClient.GetAccountByIdAsync(serviceProvider.AccountId, cancellationToken);

            var serviceProviderEmail = new EmailRequest()
            {
                RecepientEmail = serviceProviderAccount.Email,
                Message =  message,
                Subject = subject
            };

            await _bookingClient.AddBookingAsync(request, cancellationToken);

            var sendClientNotificationTask = _notificationClient.SendEmailAsync(clientEmail, cancellationToken);
            var sendProviderNotificationTask = _notificationClient.SendEmailAsync(serviceProviderEmail, cancellationToken);

            await Task.WhenAll(sendClientNotificationTask, sendProviderNotificationTask);
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsByServiceIdAsync
            (Guid serviceId, CancellationToken cancellationToken)
        {
            return await _bookingClient
                .GetBookingsByServiceIdAsync(serviceId, cancellationToken);
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsProfileIdAsync
        (Guid profileId, CancellationToken cancellationToken)
        {
            return await _bookingClient
                .GetBookingsByProfileIdAsync(profileId, cancellationToken);
        }

        public async Task UpdateBookingStatusAsync
           (UpdateBookingStatusRequest request, CancellationToken cancellationToken)
        {
            await _bookingClient.UpdateBookingStatusAsync
                (request, cancellationToken);
        }

        public async Task UpdateSlotsAsync
            (Guid serviceId, List<AddSlotRequest> request, CancellationToken cancellationToken)
        {
            await _bookingClient.UpdateSlotsAsync(serviceId, request, cancellationToken);
        }

        public async Task DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            await _bookingClient.DeleteBookingAsync(bookingId, cancellationToken);
        }
    }
}
