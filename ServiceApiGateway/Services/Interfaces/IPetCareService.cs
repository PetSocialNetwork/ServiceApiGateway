using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IPetCareService
    {
        Task<ServiceResponse> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken);
        Task<ServiceResponse> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken);
        Task<bool> DeleteServiceAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateServiceAsync(UpdateServiceRequest request, CancellationToken cancellationToken);
        Task<ICollection<ServiceWithPhotoResponse>> GetServiceByProfileIdAsync(Guid profileId, CancellationToken cancellationToken);
    }
}
