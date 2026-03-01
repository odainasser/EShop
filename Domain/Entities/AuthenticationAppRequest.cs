namespace Eshop.Domain.Entities;

/// <summary>
/// for api log in
/// </summary>
public class AuthenticationAppRequest : IId<int>
{
    public int Id { get; set; }
    public string AppID { get; set; }
    public string AppSecret { get; set; }
    public string MuseumCode { get; set; }
    public string StaffID { get; set; }
}
