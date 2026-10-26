using System.ComponentModel.DataAnnotations;

namespace Batipromo.Api.DTOs;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateMouvementRequest
{
    [Required]
    public int CaisseId { get; set; }

    [Required]
    public string Type { get; set; } = "DECAISSEMENT"; // 'ALIMENTATION', 'DECAISSEMENT'

    [Required]
    [Range(0.01, 1000000000)]
    public decimal Montant { get; set; }

    [Required]
    public string Motif { get; set; } = string.Empty;

    public string? Rubrique { get; set; }
    public string? BeneficiaireName { get; set; }
    public string? PieceJustificativeRef { get; set; }
    public string ModePaiement { get; set; } = "ESPECES";
}

public class DashboardSummaryDto
{
    public decimal TotalBudgetChantiers { get; set; }
    public decimal TotalTresorerieGlobale { get; set; }
    public decimal TotalDepensesMois { get; set; }
    public int ChantiersActifsCount { get; set; }
    public int TotalEffectifOuvriers { get; set; }
    public int AlertesCaissesCount { get; set; }
}
