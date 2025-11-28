using PetSocialNetwork.ServicePetCare;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IPetCareService
    {
        Task<ServiceResponse> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken);
        Task<ServiceResponse> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken);
        Task DeleteServiceAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateServiceAsync(UpdateServiceRequest request, CancellationToken cancellationToken);
        Task<ICollection<ServiceResponse>> GetServiceByProfileIdAsync(Guid profileId, CancellationToken cancellationToken);
    }
}
