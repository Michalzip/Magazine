// Endpoint do importowania danych z plików CSV do bazy danych
using Api.EndpointDefinitions;
using Application.Commands;
using MediatR;

namespace Api.CQRS.Commands
{
    public class ImportEndpoint : IEndpointDefinition
    {
        // Mapowanie endpointu POST do importu danych
        public void MapEndpoint(WebApplication app)
        {
            app.MapPost(
                    $"{ApiVersion.Prefix}/import",
                    async (IMediator mediator) =>
                    {
                        // Wysłanie komendy importu danych przez MediatR
                        await mediator.Send(new ImportDataCommand());
                        return Results.Ok(new { message = "Data imported successfully" });
                    }
                )
                .WithName("ImportData") // Nazwa endpointu
                .WithTags("Import") // Tag do dokumentacji
                .WithSummary("Import data from CSV files") // Krótkie podsumowanie
                .WithDescription(
                    "Imports product data, inventory, and prices from CSV files into the database. Filters products that are not wires and have shipping time <= 24 hours."
                ) // Szczegółowy opis
                .WithOpenApi(); // Dodanie do dokumentacji OpenAPI
        }
    }
}
