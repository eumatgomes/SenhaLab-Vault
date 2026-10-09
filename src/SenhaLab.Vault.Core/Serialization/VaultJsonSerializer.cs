using System.Text.Json;
using System.Text.Json.Serialization;
using SenhaLab.Vault.Core.Entries;
using SenhaLab.Vault.Core.Vault;

namespace SenhaLab.Vault.Core.Serialization;

public static class VaultJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    static VaultJsonSerializer()
    {
        Options.Converters.Add(
            new JsonStringEnumConverter(
                JsonNamingPolicy.CamelCase
            )
        );
    }

    public static string Serialize(VaultData vault)
    {
        ArgumentNullException.ThrowIfNull(vault);

        return JsonSerializer.Serialize(
            vault,
            Options
        );
    }

    public static VaultData Deserialize(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        var vault =
            JsonSerializer.Deserialize<VaultData>(
                json,
                Options
            );

        return vault
            ?? throw new JsonException(
                "O conteúdo do cofre não pôde ser desserializado."
            );
    }
}