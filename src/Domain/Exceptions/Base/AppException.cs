using System.Net;

namespace Domain.Exceptions.Base;

// Klasa bazowa dla wszystkich wyjątków aplikacji
public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public AppException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError
    )
        : base(message)
    {
        StatusCode = statusCode;
    }
}
