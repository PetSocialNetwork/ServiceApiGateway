using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServicePhoto;
using System.Runtime.CompilerServices;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetPhotoController : ControllerBase
    {
        private readonly IPetPhotoClient _petPhotoCleint;
        public PetPhotoController(IPetPhotoClient petPhotoCleint)
        {
            _petPhotoCleint = petPhotoCleint ?? throw new ArgumentException(nameof(petPhotoCleint));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<PetPhotoReponse> AddAndSetPetPhotoAsync(IFormFile file, [FromForm]Guid accountId, CancellationToken cancellationToken)
        {
            await using var fileStream = file.OpenReadStream();

            var p = new FileParameter(fileStream, "тестовое имя.jpg", file.ContentType);
            return await _petPhotoCleint.AddAndSetPetPhotoAsync(p, accountId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PetPhotoReponse>> AddPetPhotoAsync(IFormFile file, [FromForm] Guid accountId, CancellationToken cancellationToken)
        {
         
            try
            {
                await using var fileStream = file.OpenReadStream();

                var p = new FileParameter(fileStream, "тестовое имя", file.ContentType);
                return await _petPhotoCleint.AddPetPhotoAsync(p, accountId, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PetPhotoReponse>? BySearchPetPhotosAsync(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var photoResponse in await _petPhotoCleint.BySearchPetPhotosAsync(accountId, cancellationToken))
                yield return photoResponse;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetPetPhotoByIdAsync(id, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PetPhotoReponse> GetMainPetPhotoAsync([FromQuery]Guid accountId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetMainPetPhotoAsync(accountId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            await _petPhotoCleint.DeletePetPhotoAsync(photoId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.SetMainPetPhotoAsync(photoId, cancellationToken);
        }
    }
}
