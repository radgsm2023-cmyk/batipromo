using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Batipromo.Api.Data;
using Batipromo.Api.Models;

namespace Batipromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChantiersController : ControllerBase
{
    private readonly BatipromoDbContext _context;

    public ChantiersController(BatipromoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetChantiers()
    {
        var chantiers = await _context.Chantiers
            .Include(c => c.ChefChantier)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.Code,
                c.Name,
                c.Type,
                c.Wilaya,
                c.Commune,
                c.Address,
                c.Status,
                c.StartDate,
                c.EndDatePlanned,
                c.BudgetTotal,
                c.ProgressPercentage,
                c.SurfaceTotaleM2,
                c.NombreUnites,
                c.ChefChantierId,
                ChefChantierName = c.ChefChantier != null ? c.ChefChantier.Name : "Non assigné",
                EffectifCount = _context.Employees.Count(e => e.ChantierActuelId == c.Id && e.Status == "ACTIF"),
                EnginsCount = _context.Materiels.Count(m => m.ChantierActuelId == c.Id && m.Statut == "OPERATIONNEL"),
                CaisseSolde = _context.Caisses.Where(ca => ca.ChantierId == c.Id).Sum(ca => ca.Balance)
            })
            .ToListAsync();

        return Ok(chantiers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetChantier(int id)
    {
        var c = await _context.Chantiers
            .Include(ch => ch.ChefChantier)
            .Include(ch => ch.Caisses)
            .FirstOrDefaultAsync(ch => ch.Id == id);

        if (c == null) return NotFound(new { error = "Chantier introuvable." });

        return Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,CONDUCTEUR")]
    public async Task<ActionResult<Chantier>> CreateChantier([FromBody] Chantier chantier)
    {
        if (await _context.Chantiers.AnyAsync(c => c.Code == chantier.Code))
        {
            return BadRequest(new { error = $"Un chantier avec le code {chantier.Code} existe déjà." });
        }

        chantier.CreatedAt = DateTime.UtcNow;
        _context.Chantiers.Add(chantier);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetChantier), new { id = chantier.Id }, chantier);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,CONDUCTEUR")]
    public async Task<IActionResult> UpdateChantier(int id, [FromBody] Chantier chantier)
    {
        if (id != chantier.Id) return BadRequest();

        _context.Entry(chantier).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Chantiers.AnyAsync(e => e.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
}
