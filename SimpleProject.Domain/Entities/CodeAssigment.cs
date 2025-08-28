using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("CodeAssigment")]
public class CodeAssigment
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(CodeBatch))]
    public int BatchId { get; set; }
    public CodeBatch CodeBatch { get; set; } = null!;
    [ForeignKey(nameof(Collar))]
    public int CollarId { get; set; }
    public Collar Collar { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
}