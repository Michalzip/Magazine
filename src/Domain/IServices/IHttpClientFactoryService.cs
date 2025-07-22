using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public interface IHttpClientFactoryService
    {
        Task<Stream> DownloadAsync(string url);
    }
}
