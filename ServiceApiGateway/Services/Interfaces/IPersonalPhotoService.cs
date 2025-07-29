using PetSocialNetwork.ServicePhoto;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IPersonalPhotoService
    {
        Task<PersonalPhotoResponse> AddPersonalPhotoAsync(Guid profileId, IFormFile file, CancellationToken cancellationToken);
        Task DeletePersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken);
        Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync(Guid photoId, CancellationToken cancellationToken);
        Task<PersonalPhotoResponse?> GetMainPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken);
        Task<ICollection<PersonalPhotoResponse>> BySearchAsync(PersonalPhotoBySearchRequest request, CancellationToken cancellationToken);
        Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync(PersonalPhotoRequest request, CancellationToken cancellationToken);
        Task DeleteAllPersonalPhotosAsync(Guid profileId, CancellationToken cancellationToken);
    }
}
