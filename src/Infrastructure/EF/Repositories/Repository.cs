using System.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Data.Repositories;

// Generyczne repozytorium dla encji, które mają wspólne metody (CRUD, bulk insert)
public class Repository<T> : RepositoryBase<T>, IRepository<T>
    where T : class
{
    // Konstruktor z wstrzykiwaniem zależności (połączenie i kontekst EF)
    public Repository(IDbConnection dbConnection, AppDbContext dbContext)
        : base(dbConnection, dbContext) { }
}
