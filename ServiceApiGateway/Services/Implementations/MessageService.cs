using PetSocialNetwork.ServiceChat;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageClient _messageClient;
        private readonly IUserProfileClient _userProfileClient;
        public MessageService(
            IMessageClient messageClient,
            IUserProfileClient userProfileClient)
        {
            _messageClient = messageClient
                ?? throw new ArgumentNullException(nameof(messageClient));
            _userProfileClient = userProfileClient
                ?? throw new ArgumentNullException(nameof(userProfileClient));
        }

        public async Task DeleteMessageAsync
            (Guid id, CancellationToken cancellationToken)
        {
            await _messageClient.DeleteMessageAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<MessageBySearchResponse>> BySearchAsync
            (MessageRequest request, CancellationToken cancellationToken)
        {
            var messages = await _messageClient.BySearchAsync(request, cancellationToken);
        
            var messagesWithUserNames = 
                await GetMessagesWithUserProfilesAsync(messages, cancellationToken);
            return messagesWithUserNames;
        }

        public async Task<MessageBySearchResponse> GetMessageByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            var message = await _messageClient.GetMessageByIdAsync(id, cancellationToken);
            var messageResponseList = await GetMessagesWithUserProfilesAsync([message], cancellationToken);
            return messageResponseList.Single();
        }

        public async Task UpdateMessageAsync
            (UpdateMessageRequest request, CancellationToken cancellationToken)
        {
            await _messageClient.UpdateMessageAsync(request, cancellationToken);
        }

        public async Task<MessageBySearchResponse?> GetLastMessageByChatIdAsync
            (Guid chatId, CancellationToken cancellationToken)
        {
            var message = await _messageClient.GetLastMessageByChatIdAsync(chatId, cancellationToken);
            var messageResponseList = await GetMessagesWithUserProfilesAsync([message], cancellationToken);
            return messageResponseList.FirstOrDefault();
        }

        private async Task<List<MessageBySearchResponse>> GetMessagesWithUserProfilesAsync
            (ICollection<MessageResponse> messages, CancellationToken cancellationToken)
        {
            var userIds = messages.Select(m => m.UserId).Distinct().ToArray();
            var profiles = await _userProfileClient.GetUserProfilesAsync(userIds, cancellationToken);
            var profilesDict = profiles.ToDictionary(p => p.Id, p => p);

            var messagesWithUserNames = messages.Select(msg =>
            {
                profilesDict.TryGetValue(msg.UserId, out var profile);
                return new MessageBySearchResponse
                {
                    Id = msg.Id,
                    ChatId = msg.ChatId,
                    UserId = msg.UserId,
                    MessageText = msg.MessageText,
                    DateRecord = msg.DateRecord,
                    UserName = $"{profile.FirstName} {profile.LastName}"
                };
            }).ToList();
            return messagesWithUserNames;
        }
    }
}
