using PetSocialNetwork.ServiceFriend;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IFriendShipService
    {
        Task DeleteFriendAsync(FriendRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<FriendsInfoResponse>> GetSentRequestAsync(FriendBySearchRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<FriendsInfoResponse>> GetReceivedRequestAsync(FriendBySearchRequest request, CancellationToken cancellationToken);
        Task SendFriendRequestAsync(FriendRequest request, CancellationToken cancellationToken);
        Task AcceptFriendAsync(FriendRequest request, CancellationToken cancellationToken);
        Task RejectFriendAsync(FriendRequest request, CancellationToken cancellationToken);
        Task<bool> HasSentRequestAsync(FriendRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<FriendsInfoResponse>> GetFriendsWithInfoAsync(FriendBySearchRequest request, CancellationToken cancellationToken);
    }
}
