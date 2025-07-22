using System.Net;
using Domain.Exceptions.Base;

namespace Domain.Exceptions
{
    public class HttpDownloadException : AppException
    {
        public HttpDownloadException(string message)
            : base(message, HttpStatusCode.BadGateway) { }
    }
}
