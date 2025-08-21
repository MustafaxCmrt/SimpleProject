using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Dtos;
using SimpleProject.Domain.Entities;

namespace SimpleProject.WebAdmin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoundReportController : ControllerBase
{
    private readonly AppDbContext _context;

    public FoundReportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FoundReportRequest dto)
    {
        var entity = new FoundReport
        {
            CollarId = 0, 
            AnimalId = 0,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Message = dto.Message,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };

        _context.FoundReports.Add(entity);
        await _context.SaveChangesAsync();

        return Ok(entity.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FoundReportRequest dto)
    {
        var entity = await _context.FoundReports.FindAsync(id);
        if (entity == null) return NotFound();

        entity.FullName = dto.FullName;
        entity.Phone = dto.Phone;
        entity.Message = dto.Message;
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _context.FoundReports
            .Include(f => f.Animal)
            .Include(f => f.Collar)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (entity == null) return NotFound();

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.FoundReports.FindAsync(id);
        if (entity == null) return NotFound();

        _context.FoundReports.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}