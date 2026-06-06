# 🧩 Pyferium Produtos API

API REST desenvolvida em **ASP.NET Core** para gerenciamento de produtos no ecossistema **Pyferium**.

O projeto foi estruturado em camadas, separando responsabilidades entre **API**, **Aplicação**, **Domínio** e **Infraestrutura**, com foco em organização, manutenção e evolução do módulo de produtos.

---

## 📚 Sumário

- [📌 Sobre o projeto](#-sobre-o-projeto)
- [🚀 Funcionalidades](#-funcionalidades)
- [🛠️ Tecnologias utilizadas](#️-tecnologias-utilizadas)
- [🏗️ Arquitetura](#️-arquitetura)
- [📁 Estrutura de pastas](#-estrutura-de-pastas)
- [🌐 Endpoints](#-endpoints)
- [🧪 Validações e regras](#-validações-e-regras)
- [🗄️ Banco de dados](#️-banco-de-dados)
- [▶️ Como executar](#️-como-executar)
- [📖 Swagger](#-swagger)
- [📦 Versão atual](#-versão-atual)
- [🔮 Próximas melhorias](#-próximas-melhorias)
- [👨‍💻 Autor](#-autor)

---

## 📌 Sobre o projeto

O **Pyferium Produtos API** é uma API responsável por disponibilizar operações de cadastro, consulta, atualização e exclusão lógica de produtos.

Ela foi criada com uma estrutura modular, buscando separar claramente:

- entrada HTTP;
- regras de negócio;
- contratos de aplicação;
- acesso ao banco de dados;
- respostas padronizadas para consumo externo.

O objetivo é manter uma base simples, clara e extensível para evolução do módulo de produtos.

---

## 🚀 Funcionalidades

### 🧾 Produtos

- ✅ Cadastro de produtos;
- ✅ Listagem de produtos ativos;
- ✅ Consulta de produto por código;
- ✅ Atualização parcial de produto via `PATCH`;
- ✅ Exclusão lógica de produto;
- ✅ Validação de produto duplicado;
- ✅ Validação de categoria ativa;
- ✅ Controle de status por `IDTATIVO`;
- ✅ Documentação dos endpoints via Swagger/OpenAPI.

### 🗂️ Categorias

- ✅ Verificação de existência da categoria;
- ✅ Validação de categoria ativa antes de vincular ao produto.

---

## 🛠️ Tecnologias utilizadas

| Tecnologia | Finalidade |
|---|---|
| **ASP.NET Core Web API** | Construção da API REST |
| **C#** | Linguagem principal |
| **.NET** | Plataforma de desenvolvimento |
| **Dapper** | Execução de queries SQL |
| **NHibernate** | Gerenciamento de sessão/conexão |
| **MySQL** | Banco de dados relacional |
| **Swagger / OpenAPI** | Documentação dos endpoints |
| **DataAnnotations** | Validação de entrada |
| **XML Documentation** | Exibição de summaries no Swagger |

---

## 🏗️ Arquitetura

A solução utiliza uma separação em camadas inspirada em Clean Architecture, onde a camada de Aplicação define contratos e regras, enquanto a Infraestrutura implementa os detalhes de persistência. A API atua como camada de entrada HTTP e o Domínio concentra as entidades centrais do projeto.

```text
Solution 'Pyferium.Produtos'
│
├── Pyferium.Produtos.API
│   ├── Controllers
│   │   ├── CategoriasController.cs
│   │   └── ProdutosController.cs
│   │
│   ├── Middlewares
│   │   └── TratamentoExcecaoMiddleware.cs
│   │
│   ├── Properties
│   │   └── launchSettings.json
│   │
│   ├── appsettings.json
│   ├── Program.cs
│   └── Pyferium.http
│
├── Pyferium.Produtos.Aplicacao
│   ├── Categorias
│   │   └── Repositorios
│   │       └── ICategoriaRepositorio.cs
│   │
│   ├── Configuracoes
│   │   └── InjecaoDependenciaAplicacao.cs
│   │
│   └── Produtos
│       ├── Comandos
│       │   └── EditarProdutoComando.cs
│       │
│       ├── Excecoes
│       │   └── ProdutoNaoEncontradoException.cs
│       │
│       ├── Repositorios
│       │   ├── IProdutoComandoRepositorio.cs
│       │   └── IProdutoConsultaRepositorio.cs
│       │
│       ├── Requests
│       │   ├── CriarProdutoRequest.cs
│       │   └── EditarProdutoRequest.cs
│       │
│       ├── Responses
│       │   ├── ProdutoCriadoResponse.cs
│       │   ├── ProdutoEditadoResponse.cs
│       │   └── ProdutoListagemResponse.cs
│       │
│       └── Servicos
│           ├── Interfaces
│           │   ├── ICriarProdutoService.cs
│           │   ├── IDeletarProdutoService.cs
│           │   ├── IEditarProdutoService.cs
│           │   └── IListarProdutoService.cs
│           │
│           ├── CriarProdutoService.cs
│           ├── DeletarProdutoService.cs
│           ├── EditarProdutoService.cs
│           └── ListarProdutoService.cs
│
├── Pyferium.Produtos.Dominio
│   ├── Entidades
│   │   ├── Categoria.cs
│   │   ├── EntidadeBase.cs
│   │   └── Produto.cs
│   │
│   └── Enumeradores
│       └── AtivoEnum.cs
│
├── Pyferium.Produtos.Infraestrutura
│   ├── Configuracoes
│   │   └── InjecaoDependenciaInfraestrutura.cs
│   │
│   ├── Dados
│   │   └── NHibernateSessionFactory.cs
│   │
│   ├── Mapeamentos
│   │   ├── CategoriaMap.cs
│   │   └── ProdutoMap.cs
│   │
│   ├── Repositorios
│   │   ├── CategoriaRepositorio.cs
│   │   ├── ProdutoComandoRepositorio.cs
│   │   └── ProdutoConsultaRepositorio.cs
│   │
│   └── Tipos
│       └── AtivoEnumTipo.cs
│
└── Pyferium.Produtos.Aplicacao.Testes
    └── Produtos
        └── Servicos
            ├── CriarProdutoServiceTestes.cs
            ├── DeletarProdutoServiceTestes.cs
            ├── EditarProdutoServiceTestes.cs
            └── ListarProdutoServiceTestes.cs
```

---

## 🔄 Fluxo arquitetural

```text
HTTP Request
   ↓
Controller
   ↓
Service
   ↓
Repository Interface
   ↓
Repository Implementation
   ↓
Database
```

---

## 🧱 Responsabilidades por camada

| Projeto | Responsabilidade |
|---|---|
| **Pyferium.Produtos.API** | Expõe os endpoints HTTP, configura middlewares, Swagger, injeção de dependência e entrada da aplicação |
| **Pyferium.Produtos.Aplicacao** | Contém regras de aplicação, services, requests, responses, comandos, interfaces de repositórios e exceções específicas |
| **Pyferium.Produtos.Dominio** | Contém entidades, enums e conceitos centrais do negócio |
| **Pyferium.Produtos.Infraestrutura** | Implementa acesso a dados, repositórios concretos, mapeamentos NHibernate, sessão e tipos customizados |
| **Pyferium.Produtos.Aplicacao.Testes** | Contém testes automatizados dos services da camada de aplicação |

---

## 📁 Estrutura de pastas

```text
produtos-api/
│
├── Pyferium.Produtos.API/
│   ├── Controllers/
│   │   └── ProdutosController.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   └── Pyferium.Produtos.API.csproj
│
├── Pyferium.Produtos.Aplicacao/
│   ├── Produtos/
│   │   ├── Requests/
│   │   ├── Responses/
│   │   ├── Servicos/
│   │   ├── Repositorios/
│   │   ├── Excecoes/
│   │   └── Comandos/
│   │
│   └── Categorias/
│       └── Repositorios/
│
├── Pyferium.Produtos.Dominio/
│   └── Entidades/
│
├── Pyferium.Produtos.Infraestrutura/
│   └── Repositorios/
│
├── Pyferium.Produtos.Aplicacao.Testes/
│
├── README.md
└── Pyferium.Produtos.sln
```

---

## 🌐 Endpoints

Base URL local:

```text
https://localhost:{porta}/api/produtos
```

---

### ➕ Criar produto

```http
POST /api/produtos
```

Cria um novo produto vinculado a uma categoria ativa.

#### Request

```json
{
  "nomeProduto": "Parabrisa Toyota Corolla",
  "codigoCategoria": 1,
  "valorProduto": 850.75
}
```

#### Response `201 Created`

```json
{
  "codigoProduto": 1,
  "nomeProduto": "Parabrisa Toyota Corolla",
  "codigoCategoria": 1,
  "valorProduto": 850.75,
  "idtAtivo": "S"
}
```

---

### 📋 Listar produtos

```http
GET /api/produtos
```

Retorna todos os produtos ativos.

#### Response `200 OK`

```json
[
  {
    "codigoProduto": 1,
    "nomeProduto": "Parabrisa Toyota Corolla",
    "valorProduto": 850.75,
    "codigoCategoria": 1,
    "descricaoCategoria": "Vidros",
    "codigoNivel": "1",
    "idtAtivo": "S"
  }
]
```

---

### 🔎 Buscar produto por código

```http
GET /api/produtos/{codigoProduto}
```

Retorna os dados de um produto específico.

#### Exemplo

```http
GET /api/produtos/1
```

#### Response `200 OK`

```json
{
  "codigoProduto": 1,
  "nomeProduto": "Parabrisa Toyota Corolla",
  "valorProduto": 850.75,
  "codigoCategoria": 1,
  "descricaoCategoria": "Vidros",
  "codigoNivel": "1",
  "idtAtivo": "S"
}
```

---

### ✏️ Atualizar produto parcialmente

```http
PATCH /api/produtos/{codigoProduto}
```

Atualiza parcialmente os dados de um produto existente.

Esse endpoint permite enviar apenas os campos que devem ser alterados.  
Campos não enviados permanecem com os valores atuais.

#### Request

```json
{
  "nomeProduto": "Parabrisa Toyota Corolla Atualizado",
  "valorProduto": 900.00
}
```

Também é possível alterar outros campos individualmente:

```json
{
  "codigoCategoria": 2,
  "idtAtivo": "S"
}
```

#### Response `200 OK`

```json
{
  "codigoProduto": 1,
  "nomeProduto": "Parabrisa Toyota Corolla Atualizado",
  "codigoCategoria": 1,
  "valorProduto": 900.00,
  "idtAtivo": "S"
}
```

---

### 🗑️ Deletar produto

```http
DELETE /api/produtos/{codigoProduto}
```

Realiza a exclusão lógica do produto, alterando seu status para inativo.

#### Exemplo

```http
DELETE /api/produtos/1
```

#### Response `200 OK`

```json
{
  "mensagem": "Produto com código 1 excluído com sucesso."
}
```

---

## 🧪 Validações e regras

A API possui validações para garantir consistência dos dados.

### Produto

- O código do produto deve ser maior que zero;
- O nome do produto é obrigatório no cadastro;
- O nome do produto deve conter no máximo 80 caracteres;
- O nome do produto não pode conter caracteres inválidos;
- O valor do produto deve ser maior que zero;
- O status do produto deve ser `S` ou `N`;
- Não é permitido cadastrar produto ativo duplicado com o mesmo nome e categoria;
- Na atualização parcial, ao menos um campo deve ser informado.

### Categoria

- O código da categoria deve ser maior que zero;
- A categoria precisa existir;
- A categoria precisa estar ativa.

---

## 🗑️ Exclusão lógica

A exclusão de produto é feita de forma lógica.

Em vez de remover fisicamente o registro do banco de dados, o campo `IDTATIVO` é atualizado.

| Valor | Significado |
|---|---|
| `S` | Produto ativo |
| `N` | Produto inativo |

Esse padrão preserva o histórico e evita perda definitiva de dados.

---

## 🗄️ Banco de dados

A API utiliza banco de dados **MySQL**.

A connection string deve ser configurada no arquivo:

```text
Pyferium.Produtos.API/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "MySQL": "Server=localhost;Port=3306;Database=pyferium_produtos;Uid=root;Pwd=sua_senha;"
  }
}
```

> Ajuste os dados de conexão conforme o ambiente utilizado.

---

## ▶️ Como executar

### 1. Pré-requisitos

Antes de executar o projeto, instale:

- .NET SDK;
- MySQL;
- Visual Studio, Rider ou Visual Studio Code;
- Acesso a uma base MySQL configurada.

Verifique se o .NET está instalado:

```bash
dotnet --version
```

---

### 2. Restaurar dependências

Na raiz do projeto, execute:

```bash
dotnet restore
```

---

### 3. Compilar o projeto

```bash
dotnet build
```

---

### 4. Executar a API

Linux/macOS:

```bash
dotnet run --project Pyferium.Produtos.API/Pyferium.Produtos.API.csproj
```

Windows:

```cmd
dotnet run --project Pyferium.Produtos.API\Pyferium.Produtos.API.csproj
```

---

## 📖 Swagger

A documentação da API pode ser acessada pelo Swagger após a execução do projeto.

```text
https://localhost:{porta}/swagger
```

A API possui comentários XML nos endpoints, permitindo que os summaries apareçam na documentação OpenAPI.

Exemplo de configuração esperada no projeto da API:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<NoWarn>$(NoWarn);1591</NoWarn>
```

E no `Program.cs`:

```csharp
var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

options.IncludeXmlComments(xmlPath);
```

---

## 📦 Versão atual

```text
v0.2.0
```

### Histórico resumido

| Versão | Descrição |
|---|---|
| `v0.1.2` | Estrutura inicial do módulo de produtos |
| `v0.2.0` | Adição de summaries XML, melhoria da documentação Swagger, ajuste da atualização parcial para `PATCH` e correções menores no módulo de produtos |

---

## 🔮 Próximas melhorias

Melhorias previstas ou recomendadas:

- Implementar middleware global de tratamento de exceções;
- Padronizar responses de erro;
- Adicionar testes de integração para validar o comportamento dos endpoints HTTP, incluindo status code, payload de resposta, validações e fluxo entre Controller, Service e Repository;
- Avaliar criação de endpoint `PUT` para atualização completa;
- Avaliar retorno `204 No Content` no endpoint de exclusão;
- Implementar paginação na listagem de produtos;
- Adicionar filtros por categoria, nome e status;
- Melhorar logs com correlation id;
- Adicionar autenticação e autorização;
- Criar pipeline de CI/CD.

---

## 👨‍💻 Autor

Desenvolvido como parte do projeto **Pyferium Produtos API**.

---
