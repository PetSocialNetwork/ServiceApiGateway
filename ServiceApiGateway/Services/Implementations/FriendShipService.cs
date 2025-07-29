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
            var requests = await _friendShipClient.GetSentRequestAsync(request, cancellationToken);
            return await GetFriendsInfoAsync(requests, cancellationToken);
        }

        public async Task<IEnumerable<FriendsInfoResponse>> GetReceivedRequestAsync
            (FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            var requests = await _friendShipClient.GetReceivedRequestAsync(request, cancellationToken);
            return await GetFriendsInfoAsync(requests, cancellationToken);
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
            return await GetFriendsInfoAsync(friends, cancellationToken);
        }

        //рефакторинг
        private async Task<IEnumerable<FriendsInfoResponse>> GetFriendsInfoAsync
            (ICollection<FriendResponse>? friends, CancellationToken cancellationToken)
        {
            if (friends is null || friends.Count == 0)
            {
                return [];
            }

            var friendIds = friends.Select(f => f.FriendId).ToList();
            var profilesTask = _userProfileClient.GetUserProfilesAsync(friendIds, cancellationToken);
            var photosTask = _photoClient.GetMainPersonalPhotoByIdsAsync(friendIds, cancellationToken);

            await Task.WhenAll(profilesTask, photosTask);

            var profiles = await profilesTask;
            var photos = await photosTask;

            var photoDictionary = photos?.ToDictionary(p => p.ProfileId, p => p.FilePath)
                ?? [];

            Dictionary<Guid, UserProfileResponse> profileDictionary = null;
            if (profiles is not null && profiles.Count > 0)
            {
                profileDictionary = profiles.ToDictionary(p => p.Id, p => p);
            }

            var result = friends.Select(friend =>
            {
                UserProfileResponse profile = null;
                profileDictionary?.TryGetValue(friend.FriendId, out profile);

                string photoUrl = photoDictionary.GetValueOrDefault(friend.FriendId);

                return new FriendsInfoResponse
                {
                    Id = friend.FriendId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    PhotoUrl = photoUrl
                };
            }).ToList();

            return result;
        }
    }
}
