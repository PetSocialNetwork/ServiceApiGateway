using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using System.Runtime.CompilerServices;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentClient _commentClient;
        public CommentController(ICommentClient commentClient)
        {
            _commentClient = commentClient ?? throw new ArgumentNullException(nameof(commentClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidOperationException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<CommentResponse> GetById([FromQuery] Guid commentId, CancellationToken cancellationToken)
        {
            return await _commentClient.GetByIdAsync(commentId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task AddCommentAsync([FromBody] AddCommentRequest request, CancellationToken cancellationToken)
        {
            await _commentClient.AddCommentAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CommentNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteCommentAsync([FromQuery] Guid commentId, CancellationToken cancellationToken)
        {
            await _commentClient.DeleteCommentAsync(commentId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<CommentResponse> BySearchAsync([FromQuery] Guid photoId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var commentResult in await _commentClient.BySearchAsync(photoId, cancellationToken))
                yield return commentResult;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CommentNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("[action]")]
        public async Task UpdateCommentAsync([FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            await _commentClient.UpdateCommentAsync(request, cancellationToken);
        }
    }
}
