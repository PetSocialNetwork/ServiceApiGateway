using PetSocialNetwork.ServicePetCare;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class PetCareService : IPetCareService
    {
        private readonly IServiceClient _serviceClient;
        public PetCareService(IServiceClient serviceClient)
        {
           _serviceClient = serviceClient
                ?? throw new ArgumentException(nameof(serviceClient));
        }

        public async Task<ServiceResponse> AddServiceAsync
            (AddServiceRequest request, CancellationToken cancellationToken)
        {
          return await _serviceClient.AddServiceAsync(request, cancellationToken);
        }

        public async Task<ServiceResponse> GetServiceByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
           return await _serviceClient.GetServiceByIdAsync(id, cancellationToken);
        }

        public async Task DeleteServiceAsync(Guid id, CancellationToken cancellationToken)
        {
            await _serviceClient.DeleteServiceAsync(id, cancellationToken);
        }

        public async Task<ICollection<ServiceResponse>> GetServiceByProfileIdAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
           return await _serviceClient.GetServiceByProfileIdAsync(profileId, cancellationToken);
        }

        public async Task<ServiceTypeResponse> GetServiceTypeByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
           return await _serviceClient.GetServiceTypeByIdAsync (id, cancellationToken);
        }

        public async Task<ICollection<ServiceTypeResponse>> GetServiceTypesAsync
            (CancellationToken cancellationToken)
        {
            return await _serviceClient.GetServiceTypesAsync(cancellationToken);
        }
    }
}
