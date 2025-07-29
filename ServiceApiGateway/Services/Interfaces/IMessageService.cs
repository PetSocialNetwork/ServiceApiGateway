using PetSocialNetwork.ServiceChat;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IMessageService
    {
        Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<MessageBySearchResponse>> BySearchAsync(MessageRequest request, CancellationToken cancellationToken);
        Task<MessageBySearchResponse> GetMessageByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateMessageAsync(UpdateMessageRequest request, CancellationToken cancellationToken);
        Task<MessageBySearchResponse?> GetLastMessageByChatIdAsync(Guid chatId, CancellationToken cancellationToken);
    }
}
