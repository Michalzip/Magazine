using System.Net;
using Domain.Exceptions.Base;

namespace Domain.Exceptions
{
    public class FileStorageException : AppException
    {
        public FileStorageException(string message)
            : base(message, HttpStatusCode.InternalServerError) { }
    }
}
