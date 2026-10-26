using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Batipromo.Api.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("role")]
    public string Role { get; set; } = "CONDUCTEUR";

    [MaxLength(50)]
    [Column("phone")]
    public string? Phone { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "ACTIF";

    [Column("role_id")]
    public int? RoleId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("chantiers")]
public class Chantier
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("type")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("wilaya")]
    public string Wilaya { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("commune")]
    public string Commune { get; set; } = string.Empty;

    [Column("address")]
    public string? Address { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = "EN_COURS";

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date_planned")]
    public DateTime EndDatePlanned { get; set; }

    [Column("budget_total", TypeName = "decimal(15,2)")]
    public decimal BudgetTotal { get; set; }

    [Column("progress_percentage")]
    public int ProgressPercentage { get; set; }

    [Column("surface_totale_m2", TypeName = "decimal(10,2)")]
    public decimal? SurfaceTotaleM2 { get; set; }

    [Column("nombre_unites")]
    public int? NombreUnites { get; set; }

    [Column("chef_chantier_id")]
    public int? ChefChantierId { get; set; }

    [ForeignKey(nameof(ChefChantierId))]
    public User? ChefChantier { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Caisse> Caisses { get; set; } = new List<Caisse>();
}

[Table("caisses")]
public class Caisse
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("type")]
    public string Type { get; set; } = "CHANTIER"; // 'CHANTIER', 'BANQUE', 'CENTRALE'

    [Column("chantier_id")]
    public int? ChantierId { get; set; }

    [ForeignKey(nameof(ChantierId))]
    public Chantier? Chantier { get; set; }

    [MaxLength(100)]
    [Column("bank_name")]
    public string? BankName { get; set; }

    [MaxLength(100)]
    [Column("rib")]
    public string? Rib { get; set; }

    [Column("balance", TypeName = "decimal(15,2)")]
    public decimal Balance { get; set; }

    [Column("seuil_alerte", TypeName = "decimal(15,2)")]
    public decimal SeuilAlerte { get; set; } = 50000m;

    [Column("responsable_id")]
    public int? ResponsableId { get; set; }

    [ForeignKey(nameof(ResponsableId))]
    public User? Responsable { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "ACTIVE";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CaisseMouvement> Mouvements { get; set; } = new List<CaisseMouvement>();
}

[Table("caisse_mouvements")]
public class CaisseMouvement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("numero_bon")]
    public string NumeroBon { get; set; } = string.Empty;

    [Column("caisse_id")]
    public int CaisseId { get; set; }

    [ForeignKey(nameof(CaisseId))]
    public Caisse? Caisse { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("type")]
    public string Type { get; set; } = "DECAISSEMENT"; // 'ALIMENTATION', 'DECAISSEMENT', 'VIREMENT'

    [Column("montant", TypeName = "decimal(15,2)")]
    public decimal Montant { get; set; }

    [Column("date_mouvement")]
    public DateTime DateMouvement { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    [Column("motif")]
    public string Motif { get; set; } = string.Empty;

    [Column("rubrique")]
    public string? Rubrique { get; set; }

    [Column("beneficiaire_id")]
    public int? BeneficiaireId { get; set; }

    [Column("beneficiaire_name")]
    public string? BeneficiaireName { get; set; }

    [Column("piece_justificative_ref")]
    public string? PieceJustificativeRef { get; set; }

    [Column("mode_paiement")]
    public string ModePaiement { get; set; } = "ESPECES";

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public User? Author { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("employees")]
public class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("matricule")]
    public string Matricule { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("nom")]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("prenom")]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("qualification")]
    public string Qualification { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("type_contrat")]
    public string TypeContrat { get; set; } = "JOURNALIER"; // 'JOURNALIER', 'MENSUEL', 'TACHERON'

    [Column("taux_journalier", TypeName = "decimal(12,2)")]
    public decimal? TauxJournalier { get; set; }

    [Column("salaire_mensuel_base", TypeName = "decimal(12,2)")]
    public decimal? SalaireMensuelBase { get; set; }

    [MaxLength(50)]
    [Column("telephone")]
    public string? Telephone { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "ACTIF";

    [Column("chantier_actuel_id")]
    public int? ChantierActuelId { get; set; }

    [ForeignKey(nameof(ChantierActuelId))]
    public Chantier? ChantierActuel { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("materiel")]
public class Materiel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("designation")]
    public string Designation { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("categorie")]
    public string Categorie { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("propriete")]
    public string Propriete { get; set; } = "PROPRE"; // 'PROPRE', 'LOCATION'

    [Column("cout_journalier_location", TypeName = "decimal(12,2)")]
    public decimal? CoutJournalierLocation { get; set; }

    [MaxLength(50)]
    [Column("immatriculation")]
    public string? Immatriculation { get; set; }

    [Column("compteur_heures_km")]
    public decimal? CompteurHeuresKm { get; set; }

    [MaxLength(20)]
    [Column("statut")]
    public string Statut { get; set; } = "OPERATIONNEL";

    [Column("chantier_actuel_id")]
    public int? ChantierActuelId { get; set; }

    [ForeignKey(nameof(ChantierActuelId))]
    public Chantier? ChantierActuel { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("stocks_articles")]
public class StockArticle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("designation")]
    public string Designation { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("categorie")]
    public string Categorie { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("unite")]
    public string Unite { get; set; } = "UNITE";

    [Column("prix_unitaire_moyen", TypeName = "decimal(12,2)")]
    public decimal PrixUnitaireMoyen { get; set; }

    [Column("seuil_alerte_global")]
    public decimal SeuilAlerteGlobal { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
