using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("PetImage")]
public class PetImage : Entity
{
    [ForeignKey(nameof(Animal))]
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    [Required, StringLength(500)]
    public string Url { get; set; } = null!;
}