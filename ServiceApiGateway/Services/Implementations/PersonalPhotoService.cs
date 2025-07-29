using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class PersonalPhotoService : IPersonalPhotoService
    {
        private readonly IPersonalPhotoClient _personalPhotoClient;
        private readonly ICommentClient _commentClient;
        public PersonalPhotoService(
            IPersonalPhotoClient personalPhotoClient,
            ICommentClient commentClient)
        {
            _personalPhotoClient = personalPhotoClient
                ?? throw new ArgumentException(nameof(personalPhotoClient));
            _commentClient = commentClient
                ?? throw new ArgumentException(nameof(commentClient));
        }

        public async Task<PersonalPhotoResponse> AddPersonalPhotoAsync
            (Guid profileId,
            IFormFile file, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.AddPersonalPhotoAsync
                (new AddPersonalPhotoRequest()
                {
                    ProfileId = profileId,
                    FileBytes = await file.ReadBytesAsync(cancellationToken),
                    OriginalFileName = file.FileName
                }, cancellationToken);
        }

        public async Task DeletePersonalPhotoAsync
            (Guid photoId, CancellationToken cancellationToken)
        {
            await _personalPhotoClient.DeletePersonalPhotoAsync(photoId, cancellationToken);
            await _commentClient.DeleteAllCommentAsync([photoId], cancellationToken);
        }

        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync
            (Guid photoId, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.GetPersonalPhotoByIdAsync(photoId, cancellationToken);
        }

        public async Task<PersonalPhotoResponse?> GetMainPersonalPhotoAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.GetMainPersonalPhotoAsync(profileId, cancellationToken);
        }

        public async Task<ICollection<PersonalPhotoResponse>> BySearchAsync
            (PersonalPhotoBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.BySearchAsync(request, cancellationToken);
        }

        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync
            (PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.SetMainPersonalPhotoAsync(request, cancellationToken);
        }

        public async Task DeleteAllPersonalPhotosAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
            await _personalPhotoClient.DeleteAllPersonalPhotosAsync(profileId, cancellationToken);
        }
    }
}
