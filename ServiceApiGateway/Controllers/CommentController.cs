﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePhoto;
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
        private readonly IPersonalPhotoClient _photoClient;
        public CommentController(ICommentClient commentClient,
            IUserProfileClient userProfileClient,
            IPersonalPhotoClient photoClient)
        {
            _commentClient = commentClient
                ?? throw new ArgumentNullException(nameof(commentClient));
            _userProfileClient = userProfileClient
                ?? throw new ArgumentNullException(nameof(userProfileClient));
            _photoClient = photoClient
                 ?? throw new ArgumentNullException(nameof(photoClient));
        }

        [HttpGet("[action]")]
        public async Task<CommentResponse> GetById([FromQuery] Guid commentId, CancellationToken cancellationToken)
        {
            return await _commentClient.GetByIdAsync(commentId, cancellationToken);
        }

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

        [HttpDelete("[action]")]
        public async Task DeleteCommentAsync([FromQuery] Guid commentId, CancellationToken cancellationToken)
        {
            await _commentClient.DeleteCommentAsync(commentId, cancellationToken);
        }

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
            var profileTask = _userProfileClient.GetUserProfilesAsync(userIds, cancellationToken);
            var photoTask = _photoClient.GetMainPersonalPhotoByIdsAsync(userIds, cancellationToken);
            await Task.WhenAll(profileTask, photoTask);

            var profiles = await profileTask;
            var photos = await photoTask;

            var profileDictionary = profiles?.ToDictionary(p => p.Id, p => p);
            var photoDictionary = photos?.ToDictionary(p => p.ProfileId, p => p);

            var result = comments.Select(comment =>
            {
                profileDictionary.TryGetValue(comment.UserId, out var profile);
                photoDictionary.TryGetValue(comment.UserId, out var photo);

                return new CommentBySearchResponse
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    UserId = comment.UserId,
                    PhotoId = comment.PhotoId,
                    CreatedAt = comment.CreatedAt,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName,
                    PhotoUrl = photo?.FilePath
                };
            }).ToList();
            return result;
        }

        [HttpPut("[action]")]
        public async Task UpdateCommentAsync([FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            await _commentClient.UpdateCommentAsync(request, cancellationToken);
        }
    }
}
