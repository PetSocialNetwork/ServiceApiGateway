using PetSocialNetwork.ServicePhoto;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IPetPhotoService
    {
        Task<PetPhotoReponse> AddPetPhotoAsync(IFormFile file, Guid profileId, Guid petId, CancellationToken cancellationToken);
        Task<ICollection<PetPhotoReponse>> BySearchAsync(PetPhotoBySearchRequest request, CancellationToken cancellationToken);
        Task<PetPhotoReponse> GetPetPhotoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PetPhotoReponse?> GetMainPetPhotoAsync(Guid profileId, Guid petId, CancellationToken cancellationToken);
        Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken);
        Task<PetPhotoReponse> SetMainPetPhotoAsync(PetMainPhotoRequest request, CancellationToken cancellationToken);
    }
}
