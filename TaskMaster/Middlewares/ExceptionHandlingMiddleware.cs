using TaskMaster.Exceptions;

namespace TaskMaster.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ErrorOnValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = 400,
                Message = "Erro de validação.",
                Errors = ex.ErrorMessages.ToList()
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = 404,
                Message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = 400,
                Message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = 500,
                Message = "Ocorreu um erro interno no servidor.",
                Errors = [ex.Message]
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}