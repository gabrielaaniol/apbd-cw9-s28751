using Microsoft.AspNetCore.Mvc;
using WebApiMagazyn.DTOs;
using WebApiMagazyn.Services;

namespace WebApiMagazyn.Controllers;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _service;

    public WarehouseController(IWarehouseService service)
    {
        _service = service;
    }

    [HttpPost("manual")]
    public async Task<IActionResult> AddManual([FromBody] WarehouseRequestDto request)
    {
        return await _service.AddProductManuallyAsync(request);
    }

    [HttpPost("procedure")]
    public async Task<IActionResult> AddUsingProcedure([FromBody] WarehouseRequestDto request)
    {
        return await _service.AddProductUsingProcedureAsync(request);
    }
}