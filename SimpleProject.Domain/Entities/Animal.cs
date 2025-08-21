using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace SimpleProject.Domain.Entities;

[Table("Hayvan")]
public class Animal : Entity
{
    [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = null!;

        [Required, Column(TypeName = "char(1)")]
        [Display(Name = "Cinsiyet")]
        public string Cinsiyet { get; set; } = null!;

        [Required, StringLength(100)]
        [Display(Name = "Tür")]
        public string Tur { get; set; } = null!;

        [StringLength(150)]
        [Display(Name = "Konum (Adres Tarifi)")]
        public string? Konum { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        [Display(Name = "Enlem")]
        public decimal? Enlem { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        [Display(Name = "Boylam")]
        public decimal? Boylam { get; set; }

        [Required]
        [Display(Name = "Aşılı Mı?")]
        public bool AsiliMi { get; set; }

        [Required]
        [Display(Name = "Sahipli Mi?")]
        public bool SahipliMi { get; set; }

        [StringLength(120)]
        [Display(Name = "Sahip Adı")]
        public string? SahipAd { get; set; }

        [StringLength(40)]
        [Display(Name = "Sahip Telefonu")]
        public string? SahipTelefon { get; set; }

        [StringLength(500)]
        [Display(Name = "Fotoğraf URL")]
        public string? FotoUrl { get; set; }

        [Display(Name = "Konum Zamanı")]
        public DateTime? KonumZamani { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Konum Doğruluk Metre")]
        public decimal? KonumDogrulukMetre { get; set; }

        [StringLength(20)]
        [Display(Name = "Konum Kaynağı")]
        public string? KonumKaynak { get; set; }

        [StringLength(80)]
        [Display(Name = "İl")]
        public string? Il { get; set; }

        [StringLength(80)]
        [Display(Name = "İlçe")]
        public string? Ilce { get; set; }

        [StringLength(250)]
        [Display(Name = "Adres Satırı")]
        public string? AdresSatiri { get; set; }

        [Required]
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime UpdatedAt { get; set; }
}