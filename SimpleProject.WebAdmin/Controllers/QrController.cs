using Microsoft.AspNetCore.Mvc;
using SimpleProject.Domain.Dtos;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.WebAdmin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QrController :  ControllerBase 
{
    private readonly IQrService _service;

    public QrController(IQrService service)
    {
        _service = service;
    }

    [HttpGet("{code}")]
    [ProducesResponseType(typeof(PetPublicDto), 200)]
    public async Task<IActionResult> GetPet(string code, CancellationToken ct)
    {
        var pet = await _service.GetPetByCodeAsync(code, ct);
        return Ok(pet);
    }

    [HttpPost("{code}/scan")]
    [ProducesResponseType(typeof(ScanResponse), 200)]
    public async Task<IActionResult> Scan(string code, [FromBody] ScanRequest req, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var ua = Request.Headers["User-Agent"].ToString();
        var res = await _service.RegisterScanAsync(code, req, ip, ua, ct);
        return Ok(res);
    }

    [HttpPost("{code}/found")]
    public async Task<IActionResult> Found(string code, [FromBody] FoundReportRequest req, CancellationToken ct)
    {
        await _service.RegisterFoundReportAsync(code, req, ct);
        return Accepted();
    }
}