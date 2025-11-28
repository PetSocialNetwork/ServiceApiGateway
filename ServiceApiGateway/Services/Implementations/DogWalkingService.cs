using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class DogWalkingService : IDogWalkingService
    {
        private readonly IDogWalkingServiceClient _dogWalkingService;
        public DogWalkingService(IDogWalkingServiceClient dogWalkingService)
        {
            _dogWalkingService = dogWalkingService
                ?? throw new ArgumentException(nameof(dogWalkingService));
        }

        public async Task<DogWalkingServiceResponse> GetDogWalkingByServiceIdAsync
            (Guid serviceId, CancellationToken cancellationToken)
        {
            return await _dogWalkingService.GetDogWalkingByServiceIdAsync(serviceId, cancellationToken);
        }

        public async Task UpdateDogWalkingAsync
            (UpdateDogWalkingRequest request, CancellationToken cancellationToken)
        {
            await _dogWalkingService.UpdateDogWalkingAsync(request, cancellationToken);
        }
    }
}
