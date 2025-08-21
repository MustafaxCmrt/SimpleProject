using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Entities;

namespace SimpleProject.WebAdmin.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnimalController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnimalController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var animals = await _context.Set<Animal>().AsNoTracking().ToListAsync();
        return Ok(animals);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var animal = await _context.Set<Animal>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (animal is null) return NotFound();
        return Ok(animal);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Animal animal)
    {
        if (animal == null)
            return BadRequest();

        _context.Animal.Add(animal);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Animal updatedAnimal)
    {
        var animal = await _context.Animal.FindAsync(id);
        if (animal == null)
            return NotFound();
        
        animal.Ad = updatedAnimal.Ad;
        animal.Cinsiyet = updatedAnimal.Cinsiyet;
        animal.Tur = updatedAnimal.Tur;
        animal.Konum = updatedAnimal.Konum;
        animal.Enlem = updatedAnimal.Enlem;
        animal.Boylam = updatedAnimal.Boylam;
        animal.AsiliMi = updatedAnimal.AsiliMi;
        animal.SahipliMi = updatedAnimal.SahipliMi;
        animal.SahipAd = updatedAnimal.SahipAd;
        animal.SahipTelefon = updatedAnimal.SahipTelefon;
        animal.FotoUrl = updatedAnimal.FotoUrl;
        animal.KonumZamani = updatedAnimal.KonumZamani;
        animal.KonumDogrulukMetre = updatedAnimal.KonumDogrulukMetre;
        animal.KonumKaynak = updatedAnimal.KonumKaynak;
        animal.Il = updatedAnimal.Il;
        animal.Ilce = updatedAnimal.Ilce;
        animal.AdresSatiri = updatedAnimal.AdresSatiri;

        animal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(animal);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var animal = await _context.Animal.FindAsync(id);
        if (animal == null)
            return NotFound();

        _context.Animal.Remove(animal);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}