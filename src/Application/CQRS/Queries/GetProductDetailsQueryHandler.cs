using System.Data;
using Application.DTOs;
using AutoMapper;
using Dapper;
using Domain;
using Domain.Exceptions;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Queries
{
    // Handler obsługujący zapytanie o szczegóły produktu na podstawie SKU
    public class GetProductDetailsQueryHandler
        : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
    {
        // Połączenie do bazy danych wykorzystywane przez Dappera
        private readonly IDbConnection _dbConnection;

        // Konstruktor z wstrzykiwaniem zależności (DI)
        public GetProductDetailsQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Główna metoda obsługująca zapytanie o szczegóły produktu
        public async Task<ProductDetailsDto> Handle(
            GetProductDetailsQuery request,
            CancellationToken cancellationToken
        )
        {
            // Zapytanie SQL pobierające szczegóły produktu, stan magazynowy i cenę na podstawie SKU
            // Użycie JOIN-ów zapewnia, że zwracane są tylko produkty, które mają powiązane rekordy w każdej z tabel
            var sql = $"""
                                SELECT
                                    p."Name"                       AS Name,
                                    p."Ean"                        AS Ean,
                                    p."ProducerName"               AS ProducerName,
                                    p."DefaultImage"               AS DefaultImage,
                                    p."Category"                   AS Category,
                                    i."Quantity"                   AS Quantity,
                                    i."Unit"                       AS Unit,
                                    i."ShippingCost"               AS ShippingCost,
                                    pr."NetUnitPriceAfterDiscount" AS NetUnitPrice
                                FROM "Products"   p
                                INNER JOIN "Inventories" i ON p."Sku" = i."Sku"
                                INNER JOIN "Prices"      pr ON p."Sku" = pr."Sku"
                                WHERE p."Sku" = @Sku;
                """;

            // Pobranie danych z bazy i automatyczne mapowanie na DTO przez Dappera
            var result = await _dbConnection.QuerySingleOrDefaultAsync<ProductDetailsDto>(
                sql,
                new { Sku = request.Sku }
            );

            // Jeśli nie znaleziono produktu, rzucany jest wyjątek domenowy
            if (result == null)
                throw new ProductNotFoundException(
                    $"Produkt o SKU {request.Sku} nie został znaleziony."
                );

            return result;
        }
    }
}
