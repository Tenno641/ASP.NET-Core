using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.Data;
using CitiesManager.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Cors;

namespace CitiesManager.Controllers.v1;

[ApiVersion("1.0")]
[EnableCors()]
public class CitiesController : CustomControllerBase
{
    private readonly ApplicationDbContext _context;

    public CitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        return await _context.Cities
            .AsNoTracking()
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        var city = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(city => city.Id == id);

        if (city is null)
        {
            return NotFound();
        }

        return city;
    }

    [HttpPut("{id}")]
    [EndpointDescription("Update entire city entity.")]
    [Produces("application/json")]
    public async Task<IActionResult> PutCity(Guid id, City city)
    {
        if (id != city.Id)
        {
            return BadRequest();
        }

        _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<City>> PostCity(City city)
    {
        _context.Cities.Add(city);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (CityExists(city.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetCity", new { id = city.Id }, city);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city is null)
        {
            return NotFound();
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return _context.Cities.Any(e => e.Id == id);
    }
}
