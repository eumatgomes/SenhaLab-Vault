# SenhaLab Vault

Gerenciador de senhas local e open source, desenvolvido como parte do projeto SenhaLab.

> ⚠️ **Projeto em desenvolvimento**
>
> O SenhaLab Vault ainda não está pronto para uso em produção. A arquitetura e a implementação estão sendo desenvolvidas e testadas gradualmente.

## 🔐 Sobre o projeto

O **SenhaLab Vault** é um gerenciador de senhas com foco em **privacidade, segurança e armazenamento local**.

A proposta é permitir que o usuário mantenha suas credenciais em um arquivo de cofre criptografado (`.slvault`), sem depender de uma conta online ou de um servidor para armazenar os dados.

A arquitetura foi projetada com uma separação clara entre:

* aplicação
* lógica do cofre
* criptografia
* armazenamento
* interface gráfica

## 🏗️ Arquitetura

O projeto está sendo desenvolvido em **C# / .NET 8**, com interface desktop inicialmente planejada para Windows usando WPF.

```text
SenhaLab.Vault
│
├── SenhaLab.Vault.App
│   └── Interface gráfica
│
├── SenhaLab.Vault.Application
│   └── Sessão e lógica da aplicação
│
├── SenhaLab.Vault.Core
│   └── Modelo e regras do cofre
│
├── SenhaLab.Vault.Crypto
│   └── Criptografia
│
├── SenhaLab.Vault.Storage
│   └── Persistência dos arquivos
│
└── tests
    ├── Core
    ├── Crypto
    ├── Storage
    ├── Application
    └── Integration
```

## 🔒 Modelo de segurança

O projeto utiliza uma arquitetura de derivação e proteção de chaves baseada em:

```text
Senha
  │
  ▼
Argon2id
  │
  ▼
KEK
  │
  ▼
VMK
  │
  ▼
HKDF-SHA-256
  │
  ▼
Payload Key
  │
  ▼
XChaCha20-Poly1305
  │
  ▼
Cofre criptografado
```

### Primitivas planejadas

| Componente         | Tecnologia                    |
| ------------------ | ----------------------------- |
| KDF                | Argon2id                      |
| AEAD               | XChaCha20-Poly1305            |
| Derivação de chave | HKDF-SHA-256                  |
| Aleatoriedade      | CSPRNG do sistema operacional |
| Formato do cofre   | `.slvault`                    |
| Encoding           | UTF-8 / JSON canônico         |

A senha do usuário não será armazenada diretamente no arquivo do cofre.

## 📦 Formato `.slvault`

O SenhaLab Vault utiliza um formato próprio de arquivo:

```text
.slvault
```

O arquivo contém os metadados criptográficos necessários para abrir o cofre e um payload criptografado contendo os dados do usuário.

O conteúdo sensível permanece dentro do payload criptografado.

## 📋 Tipos de dados planejados

O Vault deverá suportar inicialmente:

* 🔑 Login
* 📝 Nota segura
* 💳 Cartão
* 👤 Identidade
* 📁 Pastas
* ⭐ Favoritos
* 🗑️ Lixeira

## 🛡️ Princípios do projeto

O desenvolvimento segue alguns princípios:

* **Local-first**
* Sem dependência obrigatória de servidor
* Criptografia autenticada
* Separação entre UI e criptografia
* Falha segura
* Nenhum fallback criptográfico silencioso
* Salvamento atômico do cofre
* Nenhum segredo em logs
* Testes automatizados para operações criptográficas
* Compatibilidade futura do formato do cofre

## 🚧 Status

Atualmente o projeto está na fase inicial de implementação.

### Concluído

* [x] Definição da arquitetura
* [x] Definição do modelo criptográfico
* [x] Definição do formato `.slvault`
* [x] Definição do schema inicial
* [x] Definição da máquina de estados do Vault
* [x] Estrutura inicial da solução .NET
* [x] Projeto `SenhaLab.Vault.Core`
* [x] Modelo inicial de `VaultData`
* [x] Modelo inicial de pastas, entradas e lixeira

### Em desenvolvimento

* [ ] Modelos completos dos tipos de entrada
* [ ] `ICryptoProvider`
* [ ] Implementação Argon2id
* [ ] HKDF-SHA-256
* [ ] XChaCha20-Poly1305
* [ ] Canonical Encoding
* [ ] Formato `.slvault`
* [ ] Storage
* [ ] Vault Core
* [ ] Testes criptográficos
* [ ] Interface WPF
* [ ] Auto-lock
* [ ] Clipboard seguro
* [ ] Gerador integrado
* [ ] Busca de credenciais

## 🧪 Testes

A segurança do projeto será acompanhada por testes automatizados.

Entre os cenários planejados:

* criação e abertura de cofre
* senha correta e incorreta
* corrupção do arquivo
* alteração de senha
* criptografia e descriptografia
* validação de integridade
* Unicode
* grandes volumes de dados
* cofres com grande quantidade de entradas
* falhas durante o salvamento

## 📌 Aviso

O SenhaLab Vault está em desenvolvimento e **não deve ser utilizado para armazenar senhas reais ou informações sensíveis neste estágio**.

A implementação criptográfica ainda está sendo construída e validada.

## 📄 Licença

A licença do projeto será definida posteriormente.
