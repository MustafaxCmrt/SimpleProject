using Microsoft.AspNetCore.Mvc;
using SimpleProject.Domain.Dtos.QrClaimDto;
using SimpleProject.Domain.Dtos.QROwnershipDto;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.WebAdmin.Controllers;


[ApiController]
[Route("api/qr")]
public class QrOwnershipController : ControllerBase
{
    private readonly IQrOwnershipService _ownership;

    public QrOwnershipController(IQrOwnershipService ownership)
    {
        _ownership = ownership;
    }

    [HttpPost("claim")]
    [ProducesResponseType(typeof(QrOwnershipResponse), 200)]
    public async Task<IActionResult> Claim([FromBody] QrClaimRequest req, CancellationToken ct)
    {
        var res = await _ownership.ClaimAsync(req, ct);
        return Ok(res);
    }
}