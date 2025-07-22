// Endpoint do pobierania szczegółów produktu na podstawie SKU
using Api.EndpointDefinitions;
using Application.Queries;
using MediatR;

namespace Api.CQRS.Queries
{
    public class GetProductDetailsEndpoint : IEndpointDefinition
    {
        // Mapowanie endpointu GET do pobierania szczegółów produktu
        public void MapEndpoint(WebApplication app)
        {
            app.MapGet(
                    $"{ApiVersion.Prefix}/product/{{sku}}",
                    async (string sku, IMediator mediator) =>
                    {
                        // Utworzenie zapytania o szczegóły produktu
                        var query = new GetProductDetailsQuery(sku);
                        // Wysłanie zapytania przez MediatR i pobranie wyniku
                        var result = await mediator.Send(query);
                        return Results.Ok(result);
                    }
                )
                .WithName("GetProductDetails") // Nazwa endpointu
                .WithTags("Products") // Tag do dokumentacji
                .WithSummary("Get product details by SKU") // Krótkie podsumowanie
                .WithDescription(
                    "Retrieves detailed information about a product including name, EAN, producer, category, image URL, inventory quantity, unit, net price, and shipping cost."
                ) // Szczegółowy opis
                .WithOpenApi(); // Dodanie do dokumentacji OpenAPI
        }
    }
}
