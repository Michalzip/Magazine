using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        Task BulkInsertAsync(IEnumerable<T> entities);
    }
}
