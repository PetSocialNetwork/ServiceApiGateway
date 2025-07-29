namespace Service_ApiGateway.Extensions
{
    public static class IFromFileExtension
    {
        public static async Task <byte[]> ReadBytesAsync
            (this IFormFile file, CancellationToken cancellationToken)
        {
            var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }
    }
}
