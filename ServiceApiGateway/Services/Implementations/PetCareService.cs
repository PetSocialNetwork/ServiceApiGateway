using PetSocialNetwork.ServiceBooking;
using PetSocialNetwork.ServicePetCare;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class PetCareService : IPetCareService
    {
        private readonly IServiceClient _serviceClient;
        private readonly IBookingClient _bookingClient;
        private readonly IServiceTypePhotoClient _serviceTypePhotoClient;
        public PetCareService(IServiceClient serviceClient,
            IBookingClient bookingClient,
            IServiceTypePhotoClient serviceTypePhotoClient)
        {
            _serviceClient = serviceClient
                 ?? throw new ArgumentException(nameof(serviceClient));
            _bookingClient = bookingClient
                ?? throw new ArgumentException(nameof(bookingClient));
            _serviceTypePhotoClient = serviceTypePhotoClient
                ?? throw new ArgumentException(nameof(serviceTypePhotoClient));
        }

        public async Task<ServiceResponse> AddServiceAsync
            (AddServiceRequest request, CancellationToken cancellationToken)
        {
            return await _serviceClient.AddServiceAsync(request, cancellationToken);
        }

        public async Task<bool> DeleteServiceAsync(Guid id, CancellationToken cancellationToken)
        {
            if (!await _bookingClient.IsBusySlotsExistsAsync(id, cancellationToken))
            {
                await _serviceClient.DeleteServiceAsync(id, cancellationToken);
                await _bookingClient.DeleteSlotsByServiceIdAsync(id, cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<ServiceResponse> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken)
        {
            return await _serviceClient.GetServiceByIdAsync(serviceId, cancellationToken);
        }

        public async Task UpdateServiceAsync(UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            await _serviceClient.UpdateServiceAsync(request, cancellationToken);
        }

        public async Task<ICollection<ServiceWithPhotoResponse>> GetServiceByProfileIdAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
            var services = await _serviceClient.GetServiceByProfileIdAsync(profileId, cancellationToken);
            var serviceTypeIds = services.Select(x => x.ServiceTypeId);
            var serviceTypePhotos = await _serviceTypePhotoClient
                .GetServiceTypePhotosAsync(serviceTypeIds, cancellationToken);
            var photosDictionary = serviceTypePhotos
                .GroupBy(p => p.ServiceTypeId)
                .ToDictionary(
                    g => g.Key,
                    g => g.FirstOrDefault()?.FilePath
                );

            var result = services.Select(service => new ServiceWithPhotoResponse
            {
                Id = service.Id,
                ProfileId = service.ProfileId,
                ServiceTypeId = service.ServiceTypeId,
                Description = service.Description,
                Price = service.Price,
                ServiceType = service.ServiceType,
                PhotoUrl = photosDictionary.ContainsKey(service.ServiceTypeId)
                    ? photosDictionary[service.ServiceTypeId]
                    : null
            }).ToList();

            return result;
        }
    }
}
