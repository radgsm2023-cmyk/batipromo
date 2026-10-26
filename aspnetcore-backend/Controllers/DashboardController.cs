using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Batipromo.Api.Data;
using Batipromo.Api.DTOs;

namespace Batipromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly BatipromoDbContext _context;

    public DashboardController(BatipromoDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var totalBudget = await _context.Chantiers.SumAsync(c => c.BudgetTotal);
        var totalTresorerie = await _context.Caisses.SumAsync(c => c.Balance);
        var chantiersActifs = await _context.Chantiers.CountAsync(c => c.Status != "LIVRE" && c.Status != "ANNULE");
        var totalEffectif = await _context.Employees.CountAsync(e => e.Status == "ACTIF");
        var alertesCaisses = await _context.Caisses.CountAsync(c => c.Balance <= c.SeuilAlerte);

        var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var totalDepensesMois = await _context.CaisseMouvements
            .Where(m => m.Type == "DECAISSEMENT" && m.DateMouvement >= firstDayOfMonth)
            .SumAsync(m => m.Montant);

        return Ok(new DashboardSummaryDto
        {
            TotalBudgetChantiers = totalBudget,
            TotalTresorerieGlobale = totalTresorerie,
            TotalDepensesMois = totalDepensesMois,
            ChantiersActifsCount = chantiersActifs,
            TotalEffectifOuvriers = totalEffectif,
            AlertesCaissesCount = alertesCaisses
        });
    }

    [HttpGet("kpis")]
    public async Task<ActionResult> GetKpis()
    {
        var chantiers = await _context.Chantiers
            .OrderByDescending(c => c.BudgetTotal)
            .Take(5)
            .Select(c => new
            {
                c.Name,
                c.Wilaya,
                c.BudgetTotal,
                c.ProgressPercentage
            })
            .ToListAsync();

        var caisses = await _context.Caisses
            .OrderByDescending(c => c.Balance)
            .Select(c => new
            {
                c.Name,
                c.Type,
                c.Balance,
                c.SeuilAlerte
            })
            .ToListAsync();

        return Ok(new { chantiers, caisses, currency = "DZD" });
    }
}
