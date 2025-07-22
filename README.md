# Magazine API

REST API do zarządzania produktami magazynowymi, z importem danych z plików CSV i pobieraniem szczegółów produktu po SKU.

## Wymagania
- .NET 7
- PostgreSQL (lub SQLite do testów)
- CsvHelper, MediatR, AutoMapper

## Architektura
- **CQRS + MediatR** – rozdzielenie komend i zapytań
- **Dapper + Entity Framework** – dostęp do bazy
- **AutoMapper** – mapowanie modeli na DTO
- **Swagger** – dokumentacja i testowanie API
- **Generyczny serwis CSV** – łatwe dodawanie nowych modeli
- **Generyczne repozytoria** – jeden wzorzec dla wszystkich typów danych
- **Minimal API + automatyczna rejestracja endpointów** – czysty, skalowalny kod
- **Custom middleware** – obsługa wyjątków i czytelne komunikaty błędów

## Moje podejście i decyzje architektoniczne

- **CQRS + MediatR**: Rozdzieliłem logikę zapisu i odczytu, co ułatwia testowanie, rozbudowę i pozwala na skalowanie projektu w przyszłości.
- **Generyczne repozytoria**: Zamiast powielać kod dla każdego typu danych, zastosowałem jeden interfejs i jedną klasę generyczną, co upraszcza rozwój i testowanie.
- **Minimal API + marker interface**: Endpointy rejestrują się automatycznie przez interfejs `IEndpointDefinition`, co pozwala na łatwe dodawanie nowych funkcji bez modyfikacji `Program.cs`.
- **CsvHelper**: Użyłem tej biblioteki do bezpiecznego i szybkiego mapowania plików CSV na modele domenowe.
- **AutoMapper**: Ograniczyłem powtarzalny kod przy mapowaniu modeli na DTO.
- **Custom middleware**: Dodałem własny middleware do obsługi wyjątków, aby API zwracało czytelne i spójne komunikaty błędów.
- **Konfiguracja przez appsettings.json**: Ścieżki do plików CSV i connection string do bazy są konfigurowalne bez rekompilacji.
- **Dbałość o czytelność i komentarze**: Każda kluczowa klasa i metoda jest opatrzona komentarzem.

## Co bym poprawił dalej
- Dodałbym testy jednostkowe i integracyjne (np. dla handlerów i repozytoriów).
- Wprowadziłbym paginację i filtrowanie dla endpointów zwracających kolekcje.
- Dodałbym upload plików CSV przez API oraz walidację danych wejściowych (np. FluentValidation).
- Wdrożyłbym CI/CD, monitoring i logowanie operacji biznesowych.
- Rozważyłbym wprowadzenie autoryzacji (np. JWT) i lepszą obsługę wersjonowania API.

## Endpointy

### Import danych z CSV
```
POST /api/v1/import
```
Importuje dane z plików CSV do bazy. Ścieżki do plików konfigurowalne w `appsettings.json`.

### Pobieranie szczegółów produktu
```
GET /api/v1/product/{sku}
```
Zwraca szczegóły produktu po SKU.

### Dodatkowe problemy z danymi

Zauważyłem, że w pliku z cenami (`Prices.csv`) pobierane są wszystkie produkty, natomiast w pliku z magazynem (`Inventory.csv`) znajdują się tylko produkty z czasem wysyłki 24h, a w tabeli produktów (`Products`) znajdują się produkty, które nie są bezprzewodowe i mają czas wysyłki 24h. Powoduje to, że w tabelach pojawiają się niepotrzebne dane, które nie są spójne pomiędzy źródłami.

### Znane ograniczenia importu danych

Obecnie endpoint importu CSV nie posiada mechanizmu deduplikacji – ponowne wywołanie importu spowoduje powielenie tych samych danych w bazie. W produkcyjnym systemie należałoby to obsłużyć (np. przez upsert, transakcje, sprawdzanie unikalności po kluczach biznesowych lub czyszczenie tabel przed importem).

Zostawiłem to celowo niezaimplementowane, ponieważ sposób rozwiązania zależy od wymagań biznesowych (czy import ma nadpisywać, ignorować duplikaty, czy może wersjonować dane).

## Konfiguracja
W pliku `appsettings.json`:
```
"CsvPaths": {
  "Products": "Infrastructure/data/Products.csv",
  "Inventory": "Infrastructure/data/Inventory.csv",
  "Prices": "Infrastructure/data/Prices.csv"
}
```

## Budowanie i uruchamianie aplikacji

### 1. Budowa aplikacji .NET

W katalogu głównym projektu uruchom:
```
dotnet restore
dotnet build
dotnet run 
```

### 2. Uruchomienie bazy danych PostgreSQL

Można uruchomić lokalnie PostgreSQL np. przez Docker:
```
docker run --name magazine-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=postgres -p 5432:5432 -d postgres:15
```

Dane dostępowe do bazy są skonfigurowane w pliku `appsettings.json`:
```
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"
}
```

Po uruchomieniu bazy i aplikacji API będzie dostępne pod adresem [http://localhost:5089](http://localhost:5089)

---

### Uwaga o architekturze DDD

Zastosowałem tu trochę "udawane" DDD (Domain-Driven Design), nie wydzielałem osobnych projektów (warstw) ze względu na niewielki rozmiar projektu, ale bardzo lubię tę architekturę i staram się trzymać jej zasad w podziale katalogów i kodu.

## Swagger
Po uruchomieniu: [http://localhost:5089](http://localhost:5089)

## Przykładowe zapytania
```
curl -X POST http://localhost:5089/api/v1/import
curl http://localhost:5089/api/v1/product/12345
```

## Rozwój
- Dodawanie nowych modeli CSV: wystarczy dodać model z atrybutami CsvHelper i zarejestrować w DI.
- Ścieżki do plików CSV można zmieniać w `appsettings.json` bez rekompilacji.

## Licencja
MIT 