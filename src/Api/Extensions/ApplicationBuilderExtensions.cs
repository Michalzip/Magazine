// Klasa rozszerzająca IApplicationBuilder o dodatkowe metody dla aplikacji
using Infrastructure.Data;
using Magazine.Application.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ApplicationBuilderExtensions
{
    // Metoda stosująca migracje bazy danych przy starcie aplikacji
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate(); // Automatyczne zastosowanie migracji
    }

    // Metoda rejestrująca globalny middleware do obsługi wyjątków
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionMiddleware>();
    }
}
