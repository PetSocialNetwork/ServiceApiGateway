using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class TypeService : ITypeService
    {
        private readonly IServiceTypeClient _serviceTypeClient;
        public TypeService(IServiceTypeClient serviceClient)
        {
            _serviceTypeClient = serviceClient
                 ?? throw new ArgumentException(nameof(serviceClient));
        }

        public async Task<ServiceTypeResponse> GetServiceTypeByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            return await _serviceTypeClient.GetServiceTypeByIdAsync(id, cancellationToken);
        }

        public async Task<ICollection<ServiceTypeResponse>> GetServiceTypesAsync
            (CancellationToken cancellationToken)
        {
            return await _serviceTypeClient.GetServiceTypesAsync(cancellationToken);
        }
    }
}
