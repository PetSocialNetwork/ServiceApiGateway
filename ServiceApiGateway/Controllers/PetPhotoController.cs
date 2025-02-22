﻿using Microsoft.AspNetCore.Authorization;
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

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<PetPhotoReponse> AddPetPhotoAsync(IFormFile file, [FromForm] Guid accountId, [FromForm] Guid petId, CancellationToken cancellationToken)
        {
            await using var fileStream = file.OpenReadStream();
            var photo = new FileParameter(fileStream, file.FileName, file.ContentType);
            return await _petPhotoCleint.AddPetPhotoAsync(petId, accountId, photo, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PetPhotoReponse>? BySearchPetPhotosAsync(Guid accountId, Guid petId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var photoResponse in await _petPhotoCleint.BySearchPetPhotosAsync(petId, accountId, cancellationToken))
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
        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync(Guid accountId, Guid petId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetMainPetPhotoAsync(petId, accountId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            //Транзакция
            await _petPhotoCleint.DeletePetPhotoAsync(photoId, cancellationToken);
            await _commentClient.DeleteAllCommentAsync([photoId], cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync([FromBody] PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.SetMainPetPhotoAsync(request, cancellationToken);
        }
    }
}
