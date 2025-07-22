// Klasa rozszerzająca do automatycznego mapowania wszystkich endpointów zdefiniowanych w projekcie
using System.Reflection;

namespace Api.EndpointDefinitions
{
    public static class EndpointDefinitionExtensions
    {
        // Metoda wyszukująca i mapująca wszystkie klasy implementujące IEndpointDefinition
        public static void MapEndpoint(this WebApplication app)
        {
            var endpointDefinitions = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IEndpointDefinition).IsAssignableFrom(t)
                    && !t.IsInterface
                    && !t.IsAbstract
                )
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>();

            // Mapowanie każdego endpointu na aplikację
            foreach (var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.MapEndpoint(app);
            }
        }
    }
}
