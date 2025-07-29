using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class PetPhotoService : IPetPhotoService
    {
        private readonly IPetPhotoClient _petPhotoCleint;
        private readonly ICommentClient _commentClient;
        public PetPhotoService(IPetPhotoClient petPhotoCleint, ICommentClient commentClient)
        {
            _petPhotoCleint = petPhotoCleint 
                ?? throw new ArgumentException(nameof(petPhotoCleint));
            _commentClient = commentClient 
                ?? throw new ArgumentException(nameof(commentClient));
        }

        public async Task<PetPhotoReponse> AddPetPhotoAsync
            (IFormFile file, Guid profileId, Guid petId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.AddPetPhotoAsync
               (new AddPetPhotoRequest()
               {
                   ProfileId = profileId,
                   PetId = petId,
                   FileBytes = await file.ReadBytesAsync(cancellationToken),
                   OriginalFileName = file.FileName
               }, cancellationToken);
        }

        public async Task<ICollection<PetPhotoReponse>> BySearchAsync
            (PetPhotoBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.BySearchAsync(request, cancellationToken);
        }

        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetPetPhotoByIdAsync(id, cancellationToken);
        }

        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync
            (Guid profileId, Guid petId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetMainPetPhotoAsync(petId, profileId, cancellationToken);
        }

        public async Task DeletePetPhotoAsync
            (Guid photoId, CancellationToken cancellationToken)
        {
            await _petPhotoCleint.DeletePetPhotoAsync(photoId, cancellationToken);
            await _commentClient.DeleteAllCommentAsync([photoId], cancellationToken);
        }

        public async Task<PetPhotoReponse> SetMainPetPhotoAsync
            (PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.SetMainPetPhotoAsync(request, cancellationToken);
        }
    }
}
