using PetSocialNetwork.ServiceFriend;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class FriendShipService : IFriendShipService
    {
        private readonly IFriendShipClient _friendShipClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPersonalPhotoClient _photoClient;
        public FriendShipService(IFriendShipClient friendShipClient,
            IUserProfileClient userProfileClient,
            IPersonalPhotoClient photoClient)
        {
            _friendShipClient = friendShipClient
                ?? throw new ArgumentNullException(nameof(friendShipClient));
            _userProfileClient = userProfileClient
                ?? throw new ArgumentNullException(nameof(userProfileClient));
            _photoClient = photoClient
                ?? throw new ArgumentNullException(nameof(photoClient));
        }

        public async Task DeleteFriendAsync
            (FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.DeleteFriendAsync(request, cancellationToken);
        }

        public async Task<IEnumerable<FriendsInfoResponse>> GetSentRequestAsync
            (FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            var sentRequests = await _friendShipClient.GetSentRequestAsync(request, cancellationToken);
            if (sentRequests is null || sentRequests.Count == 0)
            {
                return [];
            }

            var friendIds = sentRequests.Select(f => f.FriendId).ToList();
            return await GetFriendsInfoAsync(friendIds, cancellationToken);
        }

        public async Task<IEnumerable<FriendsInfoResponse>> GetReceivedRequestAsync
            (FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            var receivedRequests = await _friendShipClient.GetReceivedRequestAsync(request, cancellationToken);
            if (receivedRequests is null || receivedRequests.Count == 0)
            {
                return [];
            }

            var friendIds = receivedRequests.Select(f => f.FriendId).ToList();
            return await GetFriendsInfoAsync(friendIds, cancellationToken);
        }

        public async Task SendFriendRequestAsync
            (FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.SendFriendRequestAsync(request, cancellationToken);
        }

        public async Task AcceptFriendAsync
            (FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.AcceptFriendAsync(request, cancellationToken);
        }

        public async Task RejectFriendAsync
            (FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.RejectFriendAsync(request, cancellationToken);
        }

        public async Task<bool> HasSentRequestAsync
            (FriendRequest request, CancellationToken cancellationToken)
        {
            return await _friendShipClient.HasSentRequestAsync(request, cancellationToken);
        }

        public async Task<IEnumerable<FriendsInfoResponse>> GetFriendsWithInfoAsync
            (FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            var friends = await _friendShipClient.BySearchAsync(request, cancellationToken);
            if (friends is null || friends.Count == 0)
            {
                return [];
            }

            var friendIdsExcludingUserAsFriend = friends
                .Where(f => f.FriendId != request.UserId)
                .Select(f => f.FriendId);

            var friendIdsExcludingUserAsUser = friends
                .Where(f => f.UserId != request.UserId)
                .Select(f => f.UserId);

            var friendIds = friendIdsExcludingUserAsFriend
                .Union(friendIdsExcludingUserAsUser)
                .Distinct()
                .ToList();

            return await GetFriendsInfoAsync(friendIds, cancellationToken);
        }

        private async Task<IEnumerable<FriendsInfoResponse>> GetFriendsInfoAsync(List<Guid> friendIds, CancellationToken cancellationToken)
        {
            var profilesTask = _userProfileClient.GetUserProfilesAsync(friendIds, cancellationToken);
            var photosTask = _photoClient.GetMainPersonalPhotoByIdsAsync(friendIds, cancellationToken);

            await Task.WhenAll(profilesTask, photosTask);

            var profiles = await profilesTask;
            var photos = await photosTask;

            var profileDictionary = profiles.ToDictionary(p => p.Id);
            var photoDictionary = photos.ToDictionary(p => p.ProfileId, p => p.FilePath);

            var result = friendIds.Select(friendId =>
            {
                profileDictionary.TryGetValue(friendId, out var profile);
                photoDictionary.TryGetValue(friendId, out var photoUrl);

                return new FriendsInfoResponse
                {
                    Id = friendId,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName,
                    PhotoUrl = photoUrl
                };
            }).ToList();

            return result;
        }
    }
}
