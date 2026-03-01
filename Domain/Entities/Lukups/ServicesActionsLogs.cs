namespace Eshop.Domain.Entities.Lukups;

public class ServicesActionsLogs : BaseEntity, IId<long>
{
    public long Id { get; set; }

    public string ServiceName { get; set; }
    public string ActionName { get; set; }
    public string? Env { get; set; }
    public DateTime ActionAt { get; set; }
    public int? ActionBy { get; set; }
    public string? Note { get; set; }
}
