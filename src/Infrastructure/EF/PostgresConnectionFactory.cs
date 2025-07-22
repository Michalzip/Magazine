using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data
{
    public class PostgresConnectionFactory
    {
        // Connection string do bazy, pobierany z konfiguracji
        private readonly string _connectionString;

        public PostgresConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // Tworzy nowe połączenie do bazy PostgreSQL
        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
