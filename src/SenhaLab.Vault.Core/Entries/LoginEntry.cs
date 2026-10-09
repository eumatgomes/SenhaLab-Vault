namespace SenhaLab.Vault.Core.Entries;

public sealed class LoginEntry : Entry
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public List<string> Urls { get; set; } = [];

    public string Notes { get; set; } = string.Empty;
}