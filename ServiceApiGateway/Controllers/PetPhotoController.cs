using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Extensions;

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

        /// <summary>
        /// Добавляет фотографию питомца
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="profileId">Идентификатор пользователя</param>
        /// <param name="petId">Идентификатор питомца</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse> AddPetPhotoAsync(IFormFile file, [FromForm] Guid profileId, [FromForm] Guid petId, CancellationToken cancellationToken)
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

        /// <summary>
        /// Возвращает все фотографии питомца
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ICollection<PetPhotoReponse>> BySearchAsync([FromBody] PetPhotoBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.BySearchAsync(request, cancellationToken);
        }

        /// <summary>
        /// Возвращает фотографию питомца по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetPetPhotoByIdAsync(photoId, cancellationToken);
        }

        /// <summary>
        ///  Возвращает главную фотографию питомца
        /// </summary>
        /// <param name="petId">Идентификатор питомца</param>
        /// <param name="profileId">Идентификатор профиля пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync([FromQuery] Guid profileId, [FromQuery] Guid petId, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.GetMainPetPhotoAsync(petId, profileId, cancellationToken);
        }

        /// <summary>
        /// Удаляет фотографию питомца по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeletePetPhotoAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            await _petPhotoCleint.DeletePetPhotoAsync(photoId, cancellationToken);
            await _commentClient.DeleteAllCommentAsync([photoId], cancellationToken);
        }

        /// <summary>
        /// Устанавливает главную фотографию питомца
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync([FromBody] PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            return await _petPhotoCleint.SetMainPetPhotoAsync(request, cancellationToken);
        }
    }
}