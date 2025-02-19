using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [ProfileCompletionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentClient _commentClient;
        private readonly IUserProfileClient _userProfileClient;
        public CommentController(ICommentClient commentClient,
            IUserProfileClient userProfileClient)
        {
            _commentClient = commentClient
                ?? throw new ArgumentNullException(nameof(commentClient));
            _userProfileClient = userProfileClient
                ?? throw new ArgumentNullException(nameof(userProfileClient));
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
        public async Task<CommentBySearchResponse> AddCommentAsync([FromBody] AddCommentRequest request, CancellationToken cancellationToken)
        {
            //Транзакция
            var comment = await _commentClient.AddCommentAsync(request, cancellationToken);
            var profile = await _userProfileClient.GetUserProfileByIdAsync(comment.UserId);
            return new CommentBySearchResponse()
            {
                Id = comment.Id,
                Text = comment.Text,
                UserId = comment.UserId,
                PhotoId = comment.PhotoId,
                CreatedAt = comment.CreatedAt,
                FirstName = profile?.FirstName,
                LastName = profile?.LastName
            };
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
        public async Task<IEnumerable<CommentBySearchResponse>> GetAllCommentToPhotoAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            //Транзакция
            var comments = await _commentClient.GetAllCommentToPhotoAsync(photoId, cancellationToken);
            if (comments == null || comments.Count == 0)
            {
                return [];
            }

            var userIds = comments.Select(c => c.UserId).Distinct().ToList();
            var profiles = await _userProfileClient.GetUserProfilesAsync(userIds, cancellationToken);
            var profileDictionary = profiles.ToDictionary(p => p.Id, p => p);

            var result = comments.Select(comment =>
            {
                profileDictionary.TryGetValue(comment.UserId, out var profile);

                return new CommentBySearchResponse
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    UserId = comment.UserId,
                    PhotoId = comment.PhotoId,
                    CreatedAt = comment.CreatedAt,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName
                };
            }).ToList();

            return result;
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
