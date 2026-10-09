namespace SenhaLab.Vault.Core.Folders;

public sealed class Folder
{
    public Guid Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset ModifiedAt { get; set; }
}