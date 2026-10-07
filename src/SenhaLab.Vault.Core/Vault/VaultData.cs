using SenhaLab.Vault.Core.Entries;
using SenhaLab.Vault.Core.Folders;
using SenhaLab.Vault.Core.Trash;

namespace SenhaLab.Vault.Core.Vault;

public sealed class VaultData
{
    public int SchemaMajor { get; init; } = 1;

    public int SchemaMinor { get; init; } = 0;

    public VaultMetadata Vault { get; init; } = new();

    public List<Folder> Folders { get; init; } = [];

    public List<Entry> Entries { get; init; } = [];

    public List<Guid> Favorites { get; init; } = [];

    public List<TrashItem> Trash { get; init; } = [];
}