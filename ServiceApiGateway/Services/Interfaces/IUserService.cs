using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileBySearchResponse> GetUserProfileByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<UserProfileBySearchResponse> GetUserProfileByAccountIdAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteUserProfileAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteUserProfileByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
        Task<UserProfileResponse> AddUserProfileAsync(AddUserProfileRequest request, CancellationToken cancellationToken);
        Task UpdateUserProfileAsync(UpdateUserProfileRequest request, IFormFile? file, CancellationToken cancellationToken);
        Task<IEnumerable<UserProfileBySearchResponse>> FindUserProfileByNameAsync(FindUserProfileRequest request, CancellationToken cancellationToken);
    }
}
