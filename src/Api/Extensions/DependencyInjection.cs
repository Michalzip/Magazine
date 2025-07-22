// Klasa rozszerzająca IServiceCollection o rejestrację wszystkich usług projektu
using System.Data;
using Application.Commands;
using AutoMapper;
using Domain;
using Domain.IServices;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Http;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Api
{
    public static class DependencyInjection
    {
        // Metoda rozszerzająca do rejestracji wszystkich usług potrzebnych w projekcie
        public static IServiceCollection AddProjectServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Rejestracja MediatR do obsługi CQRS
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ImportDataCommand).Assembly)
            );

            // Rejestracja AutoMapper do mapowania DTO i encji
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Konfiguracja Entity Framework z użyciem PostgreSQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            // Konfiguracja fabryki połączeń dla Dappera
            services.AddScoped<PostgresConnectionFactory>();
            services.AddScoped<IDbConnection>(provider =>
            {
                var factory = provider.GetRequiredService<PostgresConnectionFactory>();
                return factory.CreateConnection();
            });

            // Rejestracja serwisów aplikacyjnych
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
            // Rejestracja klienta HTTP do pobierania plików CSV
            services.AddHttpClient(
                "CsvClient",
                client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(10); // Długi timeout na duże pliki
                }
            );

            // Rejestracja generycznego repozytorium
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Rejestracja serwisu do obsługi plików CSV
            services.AddScoped<ICsvService, CsvService>();

            // Konfiguracja Swagger/OpenAPI do dokumentacji API
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    ApiVersion.V1,
                    new OpenApiInfo
                    {
                        Title = "Magazine API",
                        Version = ApiVersion.V1,
                        Description = "API do zarządzania produktami magazynowymi",
                        Contact = new OpenApiContact
                        {
                            Name = "Magazine Team",
                            Email = "support@magazine.com"
                        }
                    }
                );

                // Dodanie tagów dla lepszej organizacji endpointów
                c.TagActionsBy(api => new[] { api.GroupName ?? "default" });
                c.DocInclusionPredicate((name, api) => true);
            });

            return services;
        }
    }
}
