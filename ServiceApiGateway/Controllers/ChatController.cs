using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceChat;
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
        public ChatController(IChatClient chatClient, IUserProfileClient userProfileClient)
        {
            _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
            _userProfileClient = userProfileClient
               ?? throw new ArgumentNullException(nameof(userProfileClient));
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
                                var chatBySearchResponse = new ChatBySearchResponse
                                {
                                    Id = chat.Id,
                                    UserId = userId,
                                    CreatedAt = chat.CreatedAt,
                                    FriendIds = friendIds,
                                    FirstName = profile.FirstName,
                                    LastName = profile.LastName
                                };

                                chatBySearchResponses.Add(chatBySearchResponse);
                            }
                        }
                    }
                }
            }
            return chatBySearchResponses;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<AddChatResponse> GetChatByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _chatClient.GetChatByIdAsync(id, cancellationToken);
        }
    }
}
