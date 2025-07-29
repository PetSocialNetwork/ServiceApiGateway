using PetSocialNetwork.ServicePet;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IPetService
    {
        Task<PetProfileResponse> AddPetProfileAsync(AddPetProfileRequest request, IFormFile file, CancellationToken cancellationToken);
        Task<PetProfileBySearchResponse> GetPetProfileByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdatePetProfileAsync(UpdatePetProfileRequest request, IFormFile? file, CancellationToken cancellationToken);
        Task DeletePetProfileAsync(Guid petId, Guid profileId, CancellationToken cancellationToken);
        Task<IEnumerable<PetProfileBySearchResponse>> GetPetProfilesAsync(Guid profileId, CancellationToken cancellationToken);
    }
}
