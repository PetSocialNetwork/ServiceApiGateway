using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceChat;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [ProfileCompletionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatClient _chatClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPersonalPhotoClient _photoClient;
        private readonly IMessageClient _messageClient;
        public ChatController(IChatClient chatClient, 
            IUserProfileClient userProfileClient,
            IPersonalPhotoClient photoClient,
            IMessageClient messageClient)
        {
            _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
            _userProfileClient = userProfileClient
               ?? throw new ArgumentNullException(nameof(userProfileClient));
            _photoClient = photoClient ?? throw new ArgumentNullException(nameof(photoClient));
            _messageClient = messageClient ?? throw new ArgumentNullException(nameof(messageClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ChatNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteChatAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _chatClient.DeleteChatAsync(id, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<AddChatResponse> AddChatAsync([FromBody] AddChatRequest request, CancellationToken cancellationToken)
        {
           return await _chatClient.AddChatAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<AddChatResponse> GetOrCreateChatAsync([FromBody] AddChatRequest request, CancellationToken cancellationToken)
        {
            return await _chatClient.GetOrCreateChatAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<AddChatResponse> GetChatByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _chatClient.GetChatByIdAsync(id, cancellationToken);
        }






        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ChatBySearchResponse>> BySearchAsync([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            var chats = await _chatClient.BySearchAsync(userId, cancellationToken);

            if (chats == null || chats.Count == 0)
            {
                return [];
            }

            var chatBySearchResponses = new List<ChatBySearchResponse>();

            foreach (var chat in chats)
            {
                var friendIds = chat.FriendIds.Where(friendId => friendId != userId).ToList();

                if (friendIds.Count != 0)
                {
                    var profiles = await _userProfileClient.GetUserProfilesAsync(friendIds, cancellationToken);

                    if (profiles != null && profiles.Count != 0)
                    {
                        foreach (var friendId in friendIds)
                        {
                            var profile = profiles.FirstOrDefault(p => p.Id == friendId);

                            if (profile != null)
                            {
                                var profileImageUrl = await _photoClient.GetMainPersonalPhotoAsync(friendId, cancellationToken);
                                var lastMessage = await _messageClient.GetLastMessageByChatIdAsync(chat.Id, cancellationToken);
                                var chatBySearchResponse = new ChatBySearchResponse
                                {
                                    Id = chat.Id,
                                    UserId = userId,
                                    CreatedAt = chat.CreatedAt,
                                    FriendIds = friendIds,
                                    FirstName = profile.FirstName,
                                    LastName = profile.LastName,
                                    PhotoUrl = profileImageUrl.FilePath,
                                    LastMessage = lastMessage?.MessageText ?? string.Empty, // Use null-conditional and null-coalescing
                                    UserName = lastMessage?.UserName ?? string.Empty
                                };

                                chatBySearchResponses.Add(chatBySearchResponse);
                            }
                        }
                    }
                }
            }
            return chatBySearchResponses;
        }
    }
}
