namespace Service_ApiGateway.Models.Responses
{
    public class MessageBySearchResponse
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string MessageText { get; set; }
        public DateTime DateRecord { get; set; }
        public string UserName { get; set; }
    }
}
