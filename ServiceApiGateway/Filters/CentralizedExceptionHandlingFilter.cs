using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Service_ApiGateway.Filters
{
    public class CentralizedExceptionHandlingFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var (message, statusCode) = TryGetUserMessageFromException(context);

            if (message != null && statusCode != 0)
            {
                context.Result = new ObjectResult(message)
                {
                    StatusCode = statusCode
                };
                context.ExceptionHandled = true;
            }
        }
        private (object?, int) TryGetUserMessageFromException(ExceptionContext context)
        {
            return context.Exception switch
            {
                ApiException ex => (ex.Response, StatusCodes.Status400BadRequest),
                Exception => ("Неизвестная ошибка!", StatusCodes.Status500InternalServerError),
                _ => (null, 0)
            };
        }
    }
}