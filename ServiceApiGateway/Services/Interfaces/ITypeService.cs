using PetSocialNetwork.ServicePetCare;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface ITypeService
    {
        Task<ServiceTypeResponse> GetServiceTypeByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ICollection<ServiceTypeResponse>> GetServiceTypesAsync(CancellationToken cancellationToken);
    }
}
