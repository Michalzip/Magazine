// Główna klasa uruchamiająca aplikację webową
using Api;
using Api.EndpointDefinitions;
using Api.Extensions;

// Tworzenie buildera aplikacji i rejestracja usług
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProjectServices(builder.Configuration);

// Budowanie aplikacji
var app = builder.Build();

// Konfiguracja środowiska deweloperskiego
if (app.Environment.IsDevelopment())
{
    // Włączenie dokumentacji Swagger tylko w trybie deweloperskim
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            $"/swagger/{ApiVersion.V1}/swagger.json",
            $"Magazine API {ApiVersion.V1}"
        );
        c.RoutePrefix = string.Empty; // Ustawienie Swagger UI jako stronę główną
    });

    // Automatyczne stosowanie migracji bazy danych
    app.ApplyMigrations();
}

// Globalna obsługa wyjątków
app.UseCustomExceptionHandler();
// Konfiguracja routingu
app.UseRouting();

// Mapowanie endpointów API
app.MapEndpoint();
// Uruchomienie aplikacji
app.Run();
