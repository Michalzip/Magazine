using System.Net;
using Domain.Exceptions.Base;

namespace Domain.Exceptions
{
    public class CsvImportException : AppException
    {
        public CsvImportException(string message)
            : base(message, HttpStatusCode.BadRequest) { }
    }
}
