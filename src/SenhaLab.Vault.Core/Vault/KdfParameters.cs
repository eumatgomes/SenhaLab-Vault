namespace SenhaLab.Vault.Core.Vault;

public sealed class KdfParameters
{
    public string Algorithm { get; init; } = "argon2id";

    public int Memory { get; init; }

    public int Iterations { get; init; }

    public int Parallelism { get; init; }

    public byte[] Salt { get; init; } = [];
}