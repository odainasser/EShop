namespace Eshop.Domain.Entities.Lukups;

public class AppModules : BaseEntity, IId<int>
{
    public AppModules()
    {
        AppModuleLookups = new HashSet<AppModuleLookups>();
    }

    public int Id { get; set; }
    public string Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? DescAr { get; set; }
    public string? DescEn { get; set; }

    public virtual ICollection<AppModuleLookups> AppModuleLookups { get; set; }
}
