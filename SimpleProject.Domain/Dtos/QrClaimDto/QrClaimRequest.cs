namespace SimpleProject.Domain.Dtos.QrClaimDto;

public class QrClaimRequest
{
    public string Code { get; set; } = null!;
    public int ToOwnerUserId { get; set; }
    public string? FriendlyName { get; set; }
    public string? AssetType { get; set; }
    public int? SubjectId { get; set; }
}