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
    public class PersonalPhotoController : ControllerBase
    {
        private readonly IPersonalPhotoClient _personalPhotoClient;
        private readonly ICommentClient _commentClient;
        public PersonalPhotoController(IPersonalPhotoClient personalPhotoClient, ICommentClient commentClient)
        {
            _personalPhotoClient = personalPhotoClient ?? throw new ArgumentException(nameof(personalPhotoClient));
            _commentClient = commentClient ?? throw new ArgumentException(nameof(commentClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<PersonalPhotoResponse>> AddPersonalPhotoAsync
            ([FromForm] Guid profileId,
            IFormFile file, CancellationToken cancellationToken)
        {
            await using var fileStream = file.OpenReadStream();
            var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
            return await _personalPhotoClient.AddPersonalPhotoAsync(profileId, photo, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePersonalPhotoAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            //Транзакция
            await _personalPhotoClient.DeletePersonalPhotoAsync(photoId, cancellationToken);
            await _commentClient.DeleteAllCommentAsync([photoId], cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.GetPersonalPhotoByIdAsync(photoId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse?> GetMainPersonalPhotoAsync([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.GetMainPersonalPhotoAsync(profileId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PersonalPhotoResponse>? BySearchPersonalPhotosAsync([FromQuery] Guid profileId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var photoResponse in await _personalPhotoClient.BySearchPersonalPhotosAsync(profileId, cancellationToken))
                yield return photoResponse;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync([FromBody] PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.SetMainPersonalPhotoAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteAllPersonalPhotosAsync([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            await _personalPhotoClient.DeleteAllPersonalPhotosAsync(profileId, cancellationToken);
        }
    }
}
