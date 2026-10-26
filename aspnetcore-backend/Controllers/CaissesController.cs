using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Batipromo.Api.Data;
using Batipromo.Api.DTOs;
using Batipromo.Api.Models;

namespace Batipromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaissesController : ControllerBase
{
    private readonly BatipromoDbContext _context;

    public CaissesController(BatipromoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetCaisses()
    {
        var caisses = await _context.Caisses
            .Include(c => c.Chantier)
            .Include(c => c.Responsable)
            .Select(c => new
            {
                c.Id,
                c.Code,
                c.Name,
                c.Type,
                c.ChantierId,
                ChantierName = c.Chantier != null ? c.Chantier.Name : "Trésorerie Centrale",
                c.BankName,
                c.Rib,
                c.Balance,
                c.SeuilAlerte,
                IsUnderAlert = c.Balance <= c.SeuilAlerte,
                c.Status,
                ResponsableName = c.Responsable != null ? c.Responsable.Name : "Non assigné"
            })
            .ToListAsync();

        return Ok(caisses);
    }

    [HttpGet("mouvements")]
    public async Task<ActionResult<IEnumerable<object>>> GetMouvements([FromQuery] int? caisseId, [FromQuery] int limit = 50)
    {
        var query = _context.CaisseMouvements
            .Include(m => m.Caisse)
            .Include(m => m.Author)
            .AsQueryable();

        if (caisseId.HasValue)
        {
            query = query.Where(m => m.CaisseId == caisseId.Value);
        }

        var list = await query
            .OrderByDescending(m => m.DateMouvement)
            .Take(limit)
            .Select(m => new
            {
                m.Id,
                m.NumeroBon,
                m.CaisseId,
                CaisseName = m.Caisse != null ? m.Caisse.Name : "",
                m.Type,
                m.Montant,
                m.DateMouvement,
                m.Motif,
                m.Rubrique,
                m.BeneficiaireName,
                m.ModePaiement,
                AuthorName = m.Author != null ? m.Author.Name : "Système"
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpPost("mouvements")]
    [Authorize(Roles = "ADMIN,CAISSIER,CONDUCTEUR")]
    public async Task<ActionResult> CreateMouvement([FromBody] CreateMouvementRequest req)
    {
        var caisse = await _context.Caisses.FindAsync(req.CaisseId);
        if (caisse == null) return NotFound(new { error = "Caisse introuvable." });

        if (req.Type == "DECAISSEMENT" && caisse.Balance < req.Montant)
        {
            return BadRequest(new { error = "Solde de caisse insuffisant pour ce décaissement." });
        }

        // Generate Bon number
        var prefix = req.Type == "ALIMENTATION" ? "BON-ALIM" : "BON-DEC";
        var randomSuffix = Random.Shared.Next(1000, 9999);
        var numeroBon = $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{randomSuffix}";

        // Update balance
        if (req.Type == "ALIMENTATION")
            caisse.Balance += req.Montant;
        else
            caisse.Balance -= req.Montant;

        var userIdClaim = User.FindFirst("userId")?.Value;
        int? authorId = int.TryParse(userIdClaim, out var uid) ? uid : null;

        var mouvement = new CaisseMouvement
        {
            NumeroBon = numeroBon,
            CaisseId = req.CaisseId,
            Type = req.Type,
            Montant = req.Montant,
            DateMouvement = DateTime.UtcNow,
            Motif = req.Motif,
            Rubrique = req.Rubrique,
            BeneficiaireName = req.BeneficiaireName,
            PieceJustificativeRef = req.PieceJustificativeRef,
            ModePaiement = req.ModePaiement,
            CreatedBy = authorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.CaisseMouvements.Add(mouvement);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, numeroBon, nouveauSolde = caisse.Balance, mouvementId = mouvement.Id });
    }
}
