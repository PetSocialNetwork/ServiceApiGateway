using PetSocialNetwork.ServicePetCare;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IDogWalkingService
    {
        Task<DogWalkingServiceResponse> GetDogWalkingByServiceIdAsync
            (Guid serviceId, CancellationToken cancellationToken);
        Task UpdateDogWalkingAsync
            (UpdateDogWalkingRequest request, CancellationToken cancellationToken);
    }
}
