namespace Web.Models.Media;

public class MediaDto
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public int EntityType { get; set; }
    public string CollectionName { get; set; } = "default";
    public string Name { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string Disk { get; set; } = "local";
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
}
