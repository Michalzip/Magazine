using System.Net;
using Domain.Exceptions.Base;

namespace Domain.Exceptions
{
    public class ProductNotFoundException : AppException
    {
        public ProductNotFoundException(string message)
            : base(message, HttpStatusCode.NotFound) { }
    }
}
