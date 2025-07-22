using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;

namespace Infrastructure.Repositories
{
    // Bazowa klasa repozytorium implementująca wspólne operacje na encjach
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        // Połączenie do bazy (np. dla Dappera)
        protected readonly IDbConnection _dbConnection;
        // Kontekst EF do operacji na encjach
        protected readonly AppDbContext _dbContext;

        protected RepositoryBase(IDbConnection dbConnection, AppDbContext dbContext)
        {
            _dbConnection = dbConnection;
            _dbContext = dbContext;
        }

        // Szybkie wstawianie wielu rekordów do bazy (lepsze niż AddRange dla dużych kolekcji)
        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            var uploader = new Npgsql.Bulk.NpgsqlBulkUploader(_dbContext);
            await uploader.InsertAsync(entities);
        }
    }
}
