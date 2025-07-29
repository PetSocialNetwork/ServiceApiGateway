using PetSocialNetwork.ServiceChat;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IChatService
    {
        Task DeleteChatAsync(Guid id, CancellationToken cancellationToken);
        Task<AddChatResponse> AddChatAsync(AddChatRequest request, CancellationToken cancellationToken);
        Task<AddChatResponse> GetOrCreateChatAsync(AddChatRequest request, CancellationToken cancellationToken);
        Task<AddChatResponse> GetChatByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ChatBySearchResponse>> BySearchAsync(ChatRequest request, CancellationToken cancellationToken);
    }
}
