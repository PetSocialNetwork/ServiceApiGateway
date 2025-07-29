using PetSocialNetwork.ServiceComments;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentResponse> GetById(Guid commentId, CancellationToken cancellationToken);
        Task<CommentBySearchResponse> AddCommentAsync(AddCommentRequest request, CancellationToken cancellationToken);
        Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken);
        Task<IEnumerable<CommentBySearchResponse>> GetAllCommentToPhotoAsync(CommentRequest request, CancellationToken cancellationToken);
        Task UpdateCommentAsync(UpdateCommentRequest request, CancellationToken cancellationToken);
    }
}
