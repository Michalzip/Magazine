// Middleware do globalnej obsługi wyjątków w aplikacji
using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Domain.Exceptions.Base;

namespace Magazine.Application.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;

    // Konstruktor przyjmujący następny delegat w potoku żądań
    public CustomExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Główna metoda obsługująca żądania HTTP
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Przekazanie żądania do kolejnego middleware
            await _next(context);
        }
        catch (AppException ex)
        {
            // Obsługa customowych wyjątków aplikacyjnych 
            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(
                new
                {
                    error = ex.Message,
                    code = (int)ex.StatusCode,
                    codeName = ex.StatusCode.ToString()
                }
            );

            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            // Obsługa nieoczekiwanych wyjątków
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(
                new { error = "Wewnętrzny błąd serwera", details = ex.Message }
            );
            await context.Response.WriteAsync(result);
        }
    }
}
