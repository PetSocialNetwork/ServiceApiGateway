using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePhoto;
using System.Runtime.CompilerServices;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPhotoController : ControllerBase
    {
        private readonly IPersonalPhotoClient _personalPhotoClient;
        public PersonalPhotoController(IPersonalPhotoClient personalPhotoClient)
        {
            _personalPhotoClient = personalPhotoClient ?? throw new ArgumentException(nameof(personalPhotoClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PersonalPhotoResponse> AddAndSetPersonalPhotoAsync(PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.AddAndSetPersonalPhotoAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PersonalPhotoResponse> AddPersonalPhotoAsync(PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.AddPersonalPhotoAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PersonalPhotoResponse>? BySearchPersonalPhotosAsync(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var photoResponse in await _personalPhotoClient.BySearchPersonalPhotosAsync(accountId, cancellationToken))
                yield return photoResponse;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.GetPersonalPhotoByIdAsync(id, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> GetMainPersonalPhotoAsync(CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.GetMainPersonalPhotoAsync(cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            await _personalPhotoClient.DeletePersonalPhotoAsync(photoId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            return await _personalPhotoClient.SetMainPersonalPhotoAsync(photoId, cancellationToken);
        }
    }
}
