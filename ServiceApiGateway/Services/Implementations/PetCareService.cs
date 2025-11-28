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

        public async Task DeleteServiceAsync(Guid id, CancellationToken cancellationToken)
        {
            await _serviceClient.DeleteServiceAsync(id, cancellationToken);
        }

        public async Task<ServiceResponse> GetServiceByIdAsync(Guid serviceId, CancellationToken cancellationToken)
        {
            return await _serviceClient.GetServiceByIdAsync(serviceId, cancellationToken);
        }

        public async Task UpdateServiceAsync(UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            await _serviceClient.UpdateServiceAsync(request, cancellationToken);
        }


        public async Task<ICollection<ServiceResponse>> GetServiceByProfileIdAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
           return await _serviceClient.GetServiceByProfileIdAsync(profileId, cancellationToken);
        }
    }
}
