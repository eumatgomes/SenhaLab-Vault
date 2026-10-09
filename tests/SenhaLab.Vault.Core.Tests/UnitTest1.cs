using SenhaLab.Vault.Core.Entries;
using SenhaLab.Vault.Core.Serialization;
using SenhaLab.Vault.Core.Vault;

namespace SenhaLab.Vault.Core.Tests;

public class VaultJsonSerializerTests
{
    [Fact]
    public void ShouldSerializeAndDeserializeLoginEntry()
    {
        var now = DateTimeOffset.UtcNow;

        var login = new LoginEntry
        {
            Id = Guid.NewGuid(),
            FolderId = null,
            Title = "GitHub",
            Username = "usuario@example.com",
            Password = "SenhaSuperSecreta!123",
            Urls =
            [
                "https://github.com"
            ],
            Notes = "Conta principal",
            CreatedAt = now,
            ModifiedAt = now
        };

        var vault = new VaultData
        {
            Vault = new VaultMetadata
            {
                Id = Guid.NewGuid(),
                Name = "Meu Cofre",
                CreatedAt = now,
                ModifiedAt = now
            },

            Entries =
            [
                login
            ]
        };

        var json =
            VaultJsonSerializer.Serialize(vault);

        Assert.Contains(
            "\"type\":\"login\"",
            json
        );

        Assert.Contains(
            "\"title\":\"GitHub\"",
            json
        );

        Assert.Contains(
            "\"username\":\"usuario@example.com\"",
            json
        );

        Assert.Contains(
            "\"password\":\"SenhaSuperSecreta!123\"",
            json
        );

        Assert.Contains(
            "\"createdAt\":",
            json
        );

        Assert.Contains(
            "\"modifiedAt\":",
            json
        );

        var restored =
            VaultJsonSerializer.Deserialize(json);

        var restoredEntry =
            Assert.IsType<LoginEntry>(
                Assert.Single(restored.Entries)
            );

        Assert.Equal(
            login.Id,
            restoredEntry.Id
        );

        Assert.Equal(
            login.Title,
            restoredEntry.Title
        );

        Assert.Equal(
            login.Username,
            restoredEntry.Username
        );

        Assert.Equal(
            login.Password,
            restoredEntry.Password
        );

        Assert.Equal(
            login.Urls,
            restoredEntry.Urls
        );

        Assert.Equal(
            login.Notes,
            restoredEntry.Notes
        );
    }


    [Fact]
    public void ShouldSerializeAndDeserializeAllEntryTypes()
    {
        var now = DateTimeOffset.UtcNow;

        var entries = new List<Entry>
        {
            new LoginEntry
            {
                Id = Guid.NewGuid(),
                Title = "GitHub",
                Username = "usuario@example.com",
                Password = "Senha123!",
                CreatedAt = now,
                ModifiedAt = now
            },

            new SecureNoteEntry
            {
                Id = Guid.NewGuid(),
                Title = "Nota segura",
                Content = "Informação confidencial",
                CreatedAt = now,
                ModifiedAt = now
            },

            new CardEntry
            {
                Id = Guid.NewGuid(),
                Title = "Cartão principal",
                CardholderName = "Nome Completo",
                Number = "4111111111111111",
                ExpiryMonth = 12,
                ExpiryYear = 2030,
                SecurityCode = "123",
                Pin = "4567",
                CreatedAt = now,
                ModifiedAt = now
            },

            new IdentityEntry
            {
                Id = Guid.NewGuid(),
                Title = "Identidade pessoal",
                FirstName = "Nome",
                LastName = "Sobrenome",
                Document = "12345678900",
                Email = "usuario@example.com",
                Phone = "11999999999",
                CreatedAt = now,
                ModifiedAt = now
            }
        };

        var vault = new VaultData
        {
            Vault = new VaultMetadata
            {
                Id = Guid.NewGuid(),
                Name = "Meu Cofre",
                CreatedAt = now,
                ModifiedAt = now
            },

            Entries = entries
        };

        var json =
            VaultJsonSerializer.Serialize(vault);

        Assert.Contains(
            "\"type\":\"login\"",
            json
        );

        Assert.Contains(
            "\"type\":\"secure_note\"",
            json
        );

        Assert.Contains(
            "\"type\":\"card\"",
            json
        );

        Assert.Contains(
            "\"type\":\"identity\"",
            json
        );

        var restored =
            VaultJsonSerializer.Deserialize(json);

        Assert.Equal(
            4,
            restored.Entries.Count
        );

        Assert.IsType<LoginEntry>(
            restored.Entries[0]
        );

        Assert.IsType<SecureNoteEntry>(
            restored.Entries[1]
        );

        Assert.IsType<CardEntry>(
            restored.Entries[2]
        );

        Assert.IsType<IdentityEntry>(
            restored.Entries[3]
        );
    }
}

public class CanonicalJsonSerializerTests
{
    [Fact]
    public void ShouldProduceSameBytesRegardlessOfPropertyOrder()
    {
        const string json1 =
            """{"name":"Meu Cofre","id":"abc","enabled":true}""";

        const string json2 =
            """{"enabled":true,"id":"abc","name":"Meu Cofre"}""";

        var result1 =
            SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                .CanonicalizeJson(json1);

        var result2 =
            SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                .CanonicalizeJson(json2);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void ShouldPreserveArrayOrder()
    {
        const string json1 = """{"items":["A","B","C"]}""";
        const string json2 = """{"items":["C","B","A"]}""";

        var result1 =
            SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                .CanonicalizeJson(json1);

        var result2 =
            SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                .CanonicalizeJson(json2);

        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void ShouldSortNestedObjectProperties()
    {
        const string json1 =
            """{"vault":{"name":"Teste","id":"123"}}""";

        const string json2 =
            """{"vault":{"id":"123","name":"Teste"}}""";

        var result1 =
            SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                .CanonicalizeJson(json1);

        var result2 =
            SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                .CanonicalizeJson(json2);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void ShouldRejectDecimalNumbers()
    {
        const string json = """{"value":1.5}""";

        Assert.Throws<System.Text.Json.JsonException>(
            () =>
                SenhaLab.Vault.Core.Serialization.CanonicalJsonSerializer
                    .CanonicalizeJson(json)
        );
    }

[Fact]
public void ShouldPreserveUnicodeStrings()
{
    const string json =
        """{"nome":"Informação confidencial 🔐"}""";

    var result =
        CanonicalJsonSerializer.CanonicalizeJson(json);

    using var document =
        System.Text.Json.JsonDocument.Parse(result);

    var restored =
        document.RootElement
            .GetProperty("nome")
            .GetString();

    Assert.Equal(
        "Informação confidencial 🔐",
        restored
    );
}


    [Fact]
    public void ShouldNotIncludeUtf8Bom()
    {
        const string json = """{"name":"SenhaLab"}""";

        var result =
            CanonicalJsonSerializer.CanonicalizeJson(json);

        Assert.False(
            result.Length >= 3 &&
            result[0] == 0xEF &&
            result[1] == 0xBB &&
            result[2] == 0xBF
        );
    }

}
