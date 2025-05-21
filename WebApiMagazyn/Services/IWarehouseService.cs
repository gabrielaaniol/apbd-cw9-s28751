using Microsoft.AspNetCore.Mvc;
using WebApiMagazyn.DTOs;

namespace WebApiMagazyn.Services;

public interface IWarehouseService
{
    Task<IActionResult> AddProductManuallyAsync(WarehouseRequestDto request);
    Task<IActionResult> AddProductUsingProcedureAsync(WarehouseRequestDto request);
}