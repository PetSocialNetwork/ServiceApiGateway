using PetSocialNetwork.ServiceChat;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatClient _chatClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPersonalPhotoClient _photoClient;
        private readonly IMessageClient _messageClient;
        public ChatService(IChatClient chatClient,
            IUserProfileClient userProfileClient,
            IPersonalPhotoClient photoClient,
            IMessageClient messageClient)
        {
            _chatClient = chatClient
                ?? throw new ArgumentNullException(nameof(chatClient));
            _userProfileClient = userProfileClient
               ?? throw new ArgumentNullException(nameof(userProfileClient));
            _photoClient = photoClient
                ?? throw new ArgumentNullException(nameof(photoClient));
            _messageClient = messageClient
                ?? throw new ArgumentNullException(nameof(messageClient));
        }

        public async Task DeleteChatAsync
            (Guid id, CancellationToken cancellationToken)
        {
            await _chatClient.DeleteChatAsync(id, cancellationToken);
        }

        public async Task<AddChatResponse> AddChatAsync
            (AddChatRequest request, CancellationToken cancellationToken)
        {
            return await _chatClient.AddChatAsync(request, cancellationToken);
        }

        public async Task<AddChatResponse> GetOrCreateChatAsync
            (AddChatRequest request, CancellationToken cancellationToken)
        {
            return await _chatClient.GetOrCreateChatAsync(request, cancellationToken);
        }

        public async Task<AddChatResponse> GetChatByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            return await _chatClient.GetChatByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ChatBySearchResponse>> BySearchAsync
            (ChatRequest request, CancellationToken cancellationToken)
        {
            var chats = await _chatClient.BySearchAsync(request, cancellationToken);
            if (chats == null || chats.Count == 0)
            {
                return [];
            }

            var responses = new List<ChatBySearchResponse>();

            foreach (var chat in chats)
            {
                var friendIds = chat.FriendIds.Where(id => id != request.UserId).ToList();

                if (friendIds.Count == 0)
                    continue;

                var profilesTask = _userProfileClient.GetUserProfilesAsync(friendIds, cancellationToken);
                var photoTasks = friendIds.ToDictionary(
                    id => id,
                    id => _photoClient.GetMainPersonalPhotoAsync(id, cancellationToken));

                var lastMessageTasks = friendIds.ToDictionary(
                    id => id,
                    id => _messageClient.GetLastMessageByChatIdAsync(chat.Id, cancellationToken));

                await Task.WhenAll(profilesTask, Task.WhenAll(photoTasks.Values), Task.WhenAll(lastMessageTasks.Values));

                var profiles = await profilesTask;
                if (profiles == null || profiles.Count == 0)
                    continue;

                foreach (var friendId in friendIds)
                {
                    var profile = profiles.FirstOrDefault(p => p.Id == friendId);
                    if (profile == null)
                        continue;

                    var profileImageUrl = await photoTasks[friendId];
                    var lastMessage = await lastMessageTasks[friendId];

                    var chatResponse = new ChatBySearchResponse
                    {
                        Id = chat.Id,
                        UserId = request.UserId,
                        CreatedAt = chat.CreatedAt,
                        FriendIds = friendIds,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        PhotoUrl = profileImageUrl?.FilePath ?? string.Empty,
                        LastMessage = lastMessage?.MessageText ?? string.Empty,
                        UserName = $"{profile.FirstName} {profile.LastName}"
                    };

                    responses.Add(chatResponse);
                }
            }

            return responses;
        }
    }
}
