namespace Eshop.Domain.Entities.Lukups;

public class AppModuleLookups : BaseEntity, IId<int>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? DescAr { get; set; }
    public string? DescEn { get; set; }
    public int SortNo { get; set; }

    #region Relations

    public string AppModuleCode { get; set; }
    public string AppLookupTypeCode { get; set; }
    public virtual AppLookupTypes AppLookupType { get; set; }
    public virtual AppModules AppModule { get; set; }

    #endregion
}
