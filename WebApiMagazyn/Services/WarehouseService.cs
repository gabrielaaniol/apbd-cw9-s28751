using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiMagazyn.DTOs;

namespace WebApiMagazyn.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IConfiguration _configuration;

    public WarehouseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IActionResult> AddProductManuallyAsync(WarehouseRequestDto request)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using var command = new SqlCommand();
        command.Connection = connection;
        await connection.OpenAsync();

        var transaction = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction)transaction;

        try
        {
            await transaction.CommitAsync();
            return new OkObjectResult("dodano poprawnie");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new BadRequestObjectResult("wystąpil błąd przy dodawaniu");
        }
    }

    public async Task<IActionResult> AddProductUsingProcedureAsync(WarehouseRequestDto request)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using var command = new SqlCommand("AddProductToWarehouse", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", request.Amount);
        command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

        await connection.OpenAsync();

        try
        {
            var result = await command.ExecuteScalarAsync();
            return new OkObjectResult(new { IdProductWarehouse = result });
        }
        catch (Exception)
        {
            return new BadRequestObjectResult("Wystąpil błąd przy procedurze");
        }
    }
}
