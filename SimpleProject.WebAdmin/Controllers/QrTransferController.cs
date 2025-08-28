using Microsoft.AspNetCore.Mvc;
using SimpleProject.Domain.Dtos.QROwnershipDto;
using SimpleProject.Domain.Dtos.QRTransferDto;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.WebAdmin.Controllers;

[ApiController]
[Route("api/qr/transfer")]
public class QrTransferController : ControllerBase
{
    private readonly IQrTransferService _transfer;

    public QrTransferController(IQrTransferService transfer)
    {
        _transfer = transfer;
    }

    [HttpPost("init")]
    [ProducesResponseType(typeof(QrTransferInitResponse), 200)]
    public async Task<IActionResult> Init([FromBody] QrTransferInitRequest req, CancellationToken ct)
    {
        var res = await _transfer.InitAsync(req, ct);
        return Ok(res);
    }

    [HttpPost("accept")]
    [ProducesResponseType(typeof(QrOwnershipResponse), 200)]
    public async Task<IActionResult> Accept([FromBody] QrTransferAcceptRequest req, CancellationToken ct)
    {
        var res = await _transfer.AcceptAsync(req, ct);
        return Ok(res);
    }
}