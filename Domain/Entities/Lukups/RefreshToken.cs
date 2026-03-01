namespace Eshop.Domain.Entities.Lukups;

[Owned]
public class RefreshToken : BaseEntity, IId<int>
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public string SectionCode { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public new bool IsActive => Revoked == null && !IsExpired;
}