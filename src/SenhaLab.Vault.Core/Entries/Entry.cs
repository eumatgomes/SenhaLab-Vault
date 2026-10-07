namespace SenhaLab.Vault.Core.Entries;

public sealed class Entry
{
    public Guid Id { get; init; }

    public string Type { get; init; } = string.Empty;

    public Guid? FolderId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset ModifiedAt { get; set; }
}