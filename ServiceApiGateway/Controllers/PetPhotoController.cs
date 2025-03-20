using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePhoto;
using System.Runtime.CompilerServices;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetPhotoController : ControllerBase
    {
        private readonly IPetPhotoClient _petPhotoCleint;
        private readonly ICommentClient _commentClient;
        public PetPhotoController(IPetPhotoClient petPhotoCleint, ICommentClient commentClient)
        {
            _petPhotoCleint = petPhotoCleint ?? throw new ArgumentException(nameof(petPhotoCleint));
            _commentClient = commentClient ?? throw new ArgumentException(nameof(commentClient));
        }

        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<PetPhotoReponse> AddPetPhotoAsync(IFormFile file, [FromForm] Guid profileId, [FromForm] Guid petId, CancellationToken cancellationToken)
        {
            await using var fileStream = file.OpenReadStream();
            var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
            return await _petPhotoCleint.AddPetPhotoAsync(petId, profileId, photo, cancellationToken);
        }

        [HttpGet("[action]")]
        public async IAsyncEnumerable<PetPhotoReponse>? BySearchAsync(Guid profileId, Guid petId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var photoResponse in await _petPhotoCleint.BySearchAsync(petId, profileId, cancellationToken))
                yield return photoResponse;
        }

        [HttpGet("[action]")]
        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetPetPhotoByIdAsync(id, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync(Guid profileId, Guid petId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetMainPetPhotoAsync(petId, profileId, cancellationToken);
        }

        [HttpDelete("[action]")]
        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            //Транзакция
            await _petPhotoCleint.DeletePetPhotoAsync(photoId, cancellationToken);
            await _commentClient.DeleteAllCommentAsync([photoId], cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync([FromBody] PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.SetMainPetPhotoAsync(request, cancellationToken);
        }
    }
}