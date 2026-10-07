namespace SenhaLab.Vault.Core.Vault;

public sealed class VaultMetadata
{
    public Guid Id { get; init; }

    public string Name { get; set; } = "Meu Cofre";

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset ModifiedAt { get; set; }
}