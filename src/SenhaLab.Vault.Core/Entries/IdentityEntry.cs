namespace SenhaLab.Vault.Core.Entries;

public sealed class IdentityEntry : Entry
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Document { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public IdentityAddress Address { get; set; } = new();

    public string Notes { get; set; } = string.Empty;
}

public sealed class IdentityAddress
{
    public string Street { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public string Complement { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
}