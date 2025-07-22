using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string fileName, Stream fileStream);
    }
}
