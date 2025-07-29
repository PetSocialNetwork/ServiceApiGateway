using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [ProfileCompletionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService
                ?? throw new ArgumentNullException(nameof(commentService));
        }

        /// <summary>
        /// Возвращает комментарий по его идентификатору
        /// </summary>
        /// <param name="commentId">Идентификатор комментария</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<CommentResponse> GetById([FromQuery] Guid commentId, CancellationToken cancellationToken)
        {
            return await _commentService.GetById(commentId, cancellationToken);
        }

        /// <summary>
        /// Добавляет комментарий
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<CommentBySearchResponse> AddCommentAsync([FromBody] AddCommentRequest request, CancellationToken cancellationToken)
        {
            return await _commentService.AddCommentAsync(request, cancellationToken);   
        }

        /// <summary>
        /// Удаляет комментарий по его идентификатору
        /// </summary>
        /// <param name="commentId">Идентификатор комментария</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteCommentAsync([FromQuery] Guid commentId, CancellationToken cancellationToken)
        {
            await _commentService.DeleteCommentAsync(commentId, cancellationToken);
        }

        /// <summary>
        /// Возвращает все коментарии к фотографии
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<CommentBySearchResponse>> GetAllCommentToPhotoAsync([FromBody] CommentRequest request, CancellationToken cancellationToken)
        {
            return await _commentService.GetAllCommentToPhotoAsync(request, cancellationToken);
        }

        /// <summary>
        /// Обновляет комментарий
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdateCommentAsync([FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            await _commentService.UpdateCommentAsync(request, cancellationToken);
        }
    }
}
