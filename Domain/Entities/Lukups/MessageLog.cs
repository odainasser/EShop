namespace Eshop.Domain.Entities.Lukups;

[Table(nameof(SmsMessageLog) + "s", Schema = SchemaConstant.Lukups)]
public class SmsMessageLog : BaseEntity, IId<long>
{
    [Key]
    public long Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public virtual ICollection<SmsMessageToLog> SmsMessageToLogs { get; set; }
    public bool IsSend { get; set; }
    public string? Error { get; set; }
}


[Table(nameof(SmsMessageToLog) + "s", Schema = SchemaConstant.Lukups)]
public class SmsMessageToLog : BaseEntity, IId<long>
{
    [Key]
    public long Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsLocal { get; set; }

    [ForeignKey(nameof(SmsMessageLog))]
    public long SmsMessageLogId { get; set; }

    public virtual SmsMessageLog SmsMessageLog { get; set; }
}