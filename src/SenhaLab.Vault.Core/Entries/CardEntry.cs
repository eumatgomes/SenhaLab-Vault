namespace SenhaLab.Vault.Core.Entries;

public sealed class CardEntry : Entry
{
    public string CardholderName { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public string SecurityCode { get; set; } = string.Empty;

    public string Pin { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}