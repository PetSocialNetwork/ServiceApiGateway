namespace Service_ApiGateway
{
    public record ErrorResponse(string Message, int? HttpStatusCode = null);
}
