// Komenda CQRS inicjująca proces importu danych z plików CSV do bazy danych
using MediatR;

namespace Application.Commands
{
    public class ImportDataCommand : IRequest<Unit> { }
}
