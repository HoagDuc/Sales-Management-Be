using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto;

namespace ptdn_net.Common.Exceptions;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            NotFoundException _ => StatusCodes.Status404NotFound,
            BadRequestException _ => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = ex.Message
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(errorResponse.ToString());
    }
}