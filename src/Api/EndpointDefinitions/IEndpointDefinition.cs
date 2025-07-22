// Interfejs definiujący kontrakt dla klas mapujących endpointy API
namespace Api.EndpointDefinitions
{
    public interface IEndpointDefinition
    {
        // Metoda do mapowania endpointów na aplikację
        void MapEndpoint(WebApplication app);
    }
}
