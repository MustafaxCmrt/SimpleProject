using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("CodeBatch")]
public class CodeBatch
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Dealer))]
    public int DealerId { get; set; }
    public Dealer Dealer { get; set; } = null!;
    [Required, StringLength(50)]
    public string BatchCode { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}