namespace Eshop.Domain.Entities.Lukups;

[Table(nameof(EmailLog) + "s", Schema = SchemaConstant.Lukups)]
public class EmailLog : BaseEntity
{
    public EmailLogForEnum For { get; set; } = EmailLogForEnum.None;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public bool HasAttachment { get; set; }

    public string Code { get; set; } = string.Empty;

    public string? ServerResponse { get; set; } = string.Empty;

    public virtual ICollection<EmailToLog> EmailToLogs { get; set; }
    public virtual ICollection<EmailCcLog> EmailCcLogs { get; set; }
    public virtual ICollection<EmailBccLog> EmailBccLogs { get; set; }
}

[Table(nameof(EmailToLog) + "s", Schema = SchemaConstant.Lukups)]
public class EmailToLog : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    [ForeignKey(nameof(EmailLog))]
    public long EmailLogId { get; set; }

    public virtual EmailLog EmailLog { get; set; }
}

[Table(nameof(EmailCcLog) + "s", Schema = SchemaConstant.Lukups)]
public class EmailCcLog : BaseEntity
{
    public string Email { get; set; }

    [ForeignKey(nameof(EmailLog))]
    public long EmailLogId { get; set; }

    public virtual EmailLog EmailLog { get; set; }
}

[Table(nameof(EmailBccLog) + "s", Schema = SchemaConstant.Lukups)]
public class EmailBccLog : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    [ForeignKey(nameof(EmailLog))]
    public long EmailLogId { get; set; }

    public virtual EmailLog EmailLog { get; set; }
}