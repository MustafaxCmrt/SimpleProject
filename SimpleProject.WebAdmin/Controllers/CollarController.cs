using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Entities;

namespace SimpleProject.WebAdmin.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CollarController : ControllerBase
{
    private readonly AppDbContext _context;

    public CollarController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var collars = await _context.Collars
            .Include(c => c.Animal)
            .Include(c => c.OwnerUser)
            .ToListAsync();
        return Ok(collars);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var collar = await _context.Collars
            .Include(c => c.Animal)
            .Include(c => c.OwnerUser)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (collar == null)
            return NotFound();

        return Ok(collar);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Collar collar)
    {
        _context.Collars.Add(collar);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = collar.Id }, collar);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Collar collar)
    {
        if (id != collar.Id)
            return BadRequest();

        _context.Entry(collar).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Collars.Any(c => c.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var collar = await _context.Collars.FindAsync(id);
        if (collar == null)
            return NotFound();

        _context.Collars.Remove(collar);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}