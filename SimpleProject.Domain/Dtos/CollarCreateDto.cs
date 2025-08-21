using System.ComponentModel.DataAnnotations;

namespace SimpleProject.Domain.Dtos;

public class CollarCreateDto : EntityDto
{
    [Required, StringLength(64)]
    public string Code { get; set; } = null!;
    [Required]
    public int AnimalId { get; set; }
    [Required]
    public int OwnerUserId { get; set; }
    public bool? IsActive { get; set; }
}