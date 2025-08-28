namespace SimpleProject.Domain.Dtos.CodeAssigmentsDtos;

public class CodeAssigmentResponse
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public int CollarId { get; set; }
    public DateTime AssignedAt { get; set; }
}