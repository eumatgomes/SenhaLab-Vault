using System.Text.Json.Serialization;

namespace SenhaLab.Vault.Core.Entries;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(LoginEntry), "login")]
[JsonDerivedType(typeof(SecureNoteEntry), "secure_note")]
[JsonDerivedType(typeof(CardEntry), "card")]
[JsonDerivedType(typeof(IdentityEntry), "identity")]
public class Entry
{
    public Guid Id { get; init; }

    public Guid? FolderId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset ModifiedAt { get; set; }
}