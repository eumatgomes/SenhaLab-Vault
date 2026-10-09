using SenhaLab.Vault.Core.Entries;

namespace SenhaLab.Vault.Core.Trash;

public sealed class TrashItem
{
    public Guid Id { get; init; }

    public string OriginalType { get; init; } = string.Empty;

    public DateTimeOffset DeletedAt { get; init; }

    public EntryData Data { get; init; } = new();
}