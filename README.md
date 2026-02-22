# CrudPedidos - Aplicação de Gerenciamento de Pedidos

[![Status](https://img.shields.io/badge/Status-Backend%20%2B%20Frontend%20%2B%20Docker%20Conclu%C3%ADdos-brightgreen)]()
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ed)]()
[![.NET](https://img.shields.io/badge/.NET-10.0-blue)]()
[![React](https://img.shields.io/badge/React-19-61dafb)]()
[![Vite](https://img.shields.io/badge/Vite-6-646cff)]()
[![Node](https://img.shields.io/badge/Node-22-339933)]()
[![License](https://img.shields.io/badge/License-MIT-green)]()

## 📋 Descrição do Projeto

Aplicação completa com backend em **.NET (API REST)** e frontend em **React** para gerenciar pedidos com CRUD completo, seguindo boas práticas de arquitetura **(Clean Architecture, DDD, SOLID, Clean Code)** e **testes unitários**.

---

## 📊 STATUS DE IMPLEMENTAÇÃO

### ✅ BACKEND - .NET API

#### **1. Estrutura Clean Architecture**

| Componente | Status | Descrição |
|-----------|--------|-----------|
| **CrudPedidos.Domain** | ✅ Concluído | Camada de domínio (entidades, value objects, interfaces) |
| **CrudPedidos.Application** | ✅ Concluído | DTOs, serviços e casos de uso |
| **CrudPedidos.Infrastructure** | ✅ Concluído | Repositórios, DbContext, migrations, configurações EF |
| **CrudPedidos.API** | ✅ Concluído | Controllers, middlewares, Program.cs |

#### **2. Entidades de Domínio**

| Entidade | Status | Detalhes |
|----------|--------|---------|
| **Pedido** | ✅ Concluído | Id, NomeCliente, EmailCliente, Pago, ValorTotal, ItensPedido |
| **ItemPedido** | ✅ Concluído | Id, IdProduto, NomeProduto, ValorUnitario, Quantidade |

#### **3. Funcionalidades CRUD**

| Endpoint | Método | Status | Detalhes |
|----------|--------|--------|---------|
| `/api/pedidos` | POST | ✅ Concluído | Criar novo pedido com itens |
| `/api/pedidos` | GET | ✅ Concluído | Listar todos os pedidos |
| `/api/pedidos/{id}` | GET | ✅ Concluído | Obter pedido específico |
| `/api/pedidos/{id}` | PUT | ✅ Concluído | Atualizar pedido |
| `/api/pedidos/{id}` | DELETE | ✅ Concluído | Remover pedido |

#### **4. Validações**

| Validação | Status | Detalhes |
|-----------|--------|---------|
| NomeCliente obrigatório | ✅ Concluído | Validação em Application Service |
| EmailCliente obrigatório | ✅ Concluído | Validação em Application Service |
| ItensPedido (mínimo 1 item) | ✅ Concluído | Validação em Application Service |
| Quantidade > 0 | ✅ Concluído | Validação em Domain Entity |
| ValorUnitario > 0 | ✅ Concluído | Validação em Domain Entity |

#### **5. Banco de Dados**

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **Entity Framework Core** | ✅ Concluído | DbContext configurado com suporte a SQL Server e InMemory |
| **Migrations** | ✅ Concluído | Migration inicial `InitialCreate` com tabelas Pedidos e ItensPedido |
| **DesignTimeDbContextFactory** | ✅ Concluído | Factory para suporte a migrações em tempo de design |
| **SQL Server/InMemory** | ✅ Concluído | Suporte para ambos com tratamento condicional em Program.cs |

#### **6. Documentação API**

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **Swagger** | ✅ Concluído | Habilitado e funcionando em `/swagger` |
| **Documentação** | ✅ Concluído | Endpoints documentados com comentários XML |

#### **7. Testes Unitários**

| Teste | Status | Detalhes |
|-------|--------|---------|
| **Testes CRUD Pedidos (Controller)** | ✅ Concluído | xUnit + Mocks (16 testes) |
| **Testes CRUD Pedidos (Service)** | ✅ Concluído | xUnit + Mocks (16 testes) |
| **Mocks de Repositórios** | ✅ Concluído | Moq Framework configurado |
| **Total de Testes** | ✅ Concluído | **32 testes passando** |

---

### ✅ FRONTEND - React

#### **1. Estrutura do Projeto**

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **React 19 + Vite 6** | ✅ Concluído | TypeScript, React Router v7, Fetch API |
| **Estrutura de Pastas** | ✅ Concluído | components, services, types |

#### **2. Funcionalidades**

| Funcionalidade | Status | Detalhes |
|----------------|--------|---------|
| **Listagem de Pedidos** | ✅ Concluído | Tabela com dados completos, badges de status |
| **Criar Pedido** | ✅ Concluído | Formulário dinâmico com adição/remoção de itens |
| **Editar Pedido** | ✅ Concluído | Carrega dados existentes e atualiza |
| **Deletar Pedido** | ✅ Concluído | Com confirmação via dialog |
| **Exibição de ValorTotal** | ✅ Concluído | Valor total exibido na listagem (calculado pela API) |

#### **3. Integração com API**

| Integração | Status | Detalhes |
|-----------|--------|---------|
| **Serviço Fetch API** | ✅ Concluído | Cliente HTTP com tratamento de erros |
| **Endpoints CRUD** | ✅ Concluído | Todos os 5 endpoints integrados |
| **Tratamento de Erros** | ✅ Concluído | Mensagens de erro exibidas ao usuário |
| **Proxy Vite** | ✅ Concluído | `/api/*` redirecionado para API .NET |
| **CORS** | ✅ Concluído | Configurado no backend para `localhost:5173` |

---

### ✅ INFRAESTRUTURA

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **Dockerfile API** | ✅ Concluído | Multi-stage build: SDK → publish → ASP.NET 10 runtime (porta 8080) |
| **Dockerfile Frontend** | ✅ Concluído | Multi-stage build: Node 22 → Vite build → Nginx serve |
| **nginx.conf** | ✅ Concluído | SPA routing + proxy reverso `/api/` → container API |
| **docker-compose.yml** | ✅ Concluído | 3 serviços: SQL Server 2022 + API .NET + Frontend Nginx |
| **.dockerignore** | ✅ Concluído | API e Frontend com ignores otimizados |
| **Healthcheck SQL** | ✅ Concluído | API aguarda SQL Server ficar saudável antes de iniciar |
| **Migration automática** | ✅ Concluído | EF Core aplica migrations ao iniciar em Production |
| **Deploy Cloud** | ⏳ Opcional | Não deployado em Azure/AWS/Heroku |

---

## 🏗️ ARQUITETURA DO PROJETO

### Estrutura de Diretórios

```
CrudPedidos/
├── docker-compose.yml               # Orquestra API + Frontend + SQL Server
├── .dockerignore                    # Ignora bin/, obj/, node_modules/, tests
├── CrudPedidos.Domain/              # Camada de Domínio
│   ├── Entities/                    # Entidades (Pedido, ItemPedido)
│   ├── ValueObjects/                # Value Objects
│   └── Interfaces/                  # Interfaces de repositório
├── CrudPedidos.Application/         # Camada de Aplicação
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Services/                    # Lógica de negócio
│   ├── Interfaces/                  # Interfaces de serviços
│   └── Mappings/                    # AutoMapper profiles
├── CrudPedidos.Infrastructure/      # Camada de Infraestrutura
│   ├── Data/                        # DbContext
│   ├── Repositories/                # Implementações de repositório
│   ├── Migrations/                  # EF Core Migrations
│   └── DependencyInjection/         # Configuração de DI
├── CrudPedidos.API/                 # Camada de Apresentação
│   ├── Controllers/                 # Controllers da API
│   ├── Dockerfile                   # Multi-stage build .NET 10
│   └── Program.cs                   # Configuração inicial
├── CrudPedidos.Tests/               # Testes Unitários
│   ├── Controllers/                 # Testes de controller
│   └── Services/                    # Testes de serviço
└── frontend/                        # Frontend React + TypeScript + Vite
    ├── Dockerfile                   # Multi-stage build Node 22 + Nginx
    ├── nginx.conf                   # SPA routing + proxy reverso /api/
    ├── .dockerignore                # Ignora node_modules/, dist/
    ├── index.html                   # Entry point HTML
    ├── vite.config.ts               # Configuração Vite (proxy, porta)
    ├── tsconfig.json                # Configuração TypeScript
    ├── package.json                 # Dependências React 19, Vite 6
    └── src/
        ├── main.tsx                 # Entry point React
        ├── App.tsx                  # Rotas e layout principal
        ├── App.css                  # Estilos globais e botões
        ├── index.css                # Reset CSS
        ├── components/              # Componentes React
        │   ├── PedidoList.tsx        # Listagem de pedidos (tabela)
        │   ├── PedidoList.css
        │   ├── PedidoForm.tsx        # Formulário criar/editar pedido
        │   └── PedidoForm.css
        ├── services/
        │   └── api.ts               # Cliente HTTP (Fetch API)
        └── types/
            └── pedido.ts            # Interfaces TypeScript (DTOs)
```

---

## 🛠️ TECNOLOGIAS

### Backend
- **Framework**: .NET 10.0
- **ORM**: Entity Framework Core
- **Banco de Dados**: SQL Server (ou InMemory para dev)
- **API Documentation**: Swagger/OpenAPI
- **Testes**: xUnit + Moq
- **Padrões**: Clean Architecture, DDD, SOLID

### Frontend
- **Framework**: React 19
- **Build Tool**: Vite 6
- **Linguagem**: TypeScript 5.8
- **HTTP Client**: Fetch API nativa
- **Roteamento**: React Router v7
- **Estilos**: CSS puro

### Infraestrutura
- **Containerização**: Docker & Docker Compose
- **Web Server**: Nginx (frontend em produção)
- **Banco de Dados Docker**: SQL Server 2022
- **Cloud**: Azure/AWS/Heroku (opcional)

---

## 📋 REQUISITOS

- **.NET 10.0** ou superior
- **Node.js 22.0** ou superior
- **npm 10.0** ou superior
- **SQL Server 2019+** (ou InMemory para dev)
- **Docker** (opcional, para containerização)

---

## 🚀 COMEÇANDO

### 1. Clonar o Repositório

```bash
git clone https://github.com/diogenesb6/CrudPedidos.git
cd CrudPedidos
```

### 2. Configurar Backend

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations
dotnet ef database update -p CrudPedidos.Infrastructure -s CrudPedidos.API

# Executar a API
dotnet run --project CrudPedidos.API
```

**URL da API**: `http://localhost:5234`  
**Swagger**: `http://localhost:5234/swagger`

#### Criar Novas Migrations

```bash
# Adicionar nova migration
dotnet ef migrations add NomeDaMigration -p CrudPedidos.Infrastructure -s CrudPedidos.API

# Remover última migration
dotnet ef migrations remove -p CrudPedidos.Infrastructure -s CrudPedidos.API
```

### 3. Configurar Frontend

```bash
cd frontend

# Instalar dependências
npm install

# Iniciar servidor de desenvolvimento
npm run dev
```

**URL do Frontend**: `http://localhost:5173`

> ⚠️ A API precisa estar rodando simultaneamente para o frontend funcionar. O Vite faz proxy automático de `/api/*` para `http://localhost:5234`.

### 4. Executar com Docker Compose

```bash
docker-compose up --build
```

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Frontend** | `http://localhost:5173` | React via Nginx (proxy reverso para API) |
| **API** | `http://localhost:5234` | ASP.NET 10 |
| **Swagger** | `http://localhost:5234/swagger` | Documentação interativa |
| **SQL Server** | `localhost:1433` | Usuário: `sa` / Senha: `CrudPedidos@2025` |

> A API aguarda o SQL Server ficar saudável (healthcheck) antes de iniciar e aplica as migrations automaticamente.

---

## 🧪 TESTES

### Executar Testes Unitários

```bash
# Todos os testes
dotnet test

# Apenas testes do CrudPedidos.Tests
dotnet test CrudPedidos.Tests
```

---

## 🗄️ BANCO DE DADOS

### Schema - Tabelas

#### **Tabela: Pedidos**

| Coluna | Tipo | Restrições | Descrição |
|--------|------|-----------|-----------|
| **Id** | int | PK, Auto-increment | Identificador único do pedido |
| **NomeCliente** | nvarchar(255) | NOT NULL | Nome do cliente |
| **EmailCliente** | nvarchar(255) | NOT NULL | Email do cliente |
| **Pago** | bit | NOT NULL, DEFAULT: 0 | Status de pagamento |
| **ValorTotal** | decimal(18,2) | NOT NULL | Valor total do pedido (soma dos itens) |
| **DataCriacao** | datetime2 | NOT NULL, DEFAULT: GETUTCDATE() | Data de criação do pedido |
| **DataAtualizacao** | datetime2 | NULL | Data da última atualização |

#### **Tabela: ItensPedido**

| Coluna | Tipo | Restrições | Descrição |
|--------|------|-----------|-----------|
| **Id** | int | PK, Auto-increment | Identificador único do item |
| **IdProduto** | int | NOT NULL | ID externo do produto |
| **NomeProduto** | nvarchar(255) | NOT NULL | Nome do produto |
| **ValorUnitario** | decimal(18,2) | NOT NULL | Preço unitário do produto |
| **Quantidade** | int | NOT NULL | Quantidade solicitada |
| **PedidoId** | int | NOT NULL, FK → Pedidos.Id | Chave estrangeira para Pedido |

### Relacionamento

- **One-to-Many**: Um Pedido pode ter múltiplos ItensPedido
- **Delete Cascade**: Ao deletar um Pedido, todos seus itens são removidos automaticamente

### Migrations Aplicadas

#### ✅ InitialCreate
- Criação das tabelas `Pedidos` e `ItensPedido`
- Configuração de chaves primárias e estrangeiras
- Índices para melhor desempenho

---

## 📝 API - Endpoints

### Criar Pedido
```http
POST /api/pedidos
Content-Type: application/json

{
  "nomeCliente": "João Silva",
  "emailCliente": "joao@example.com",
  "pago": false,
  "itensPedido": [
    {
      "idProduto": 1,
      "nomeProduto": "Produto A",
      "valorUnitario": 100.00,
      "quantidade": 2
    }
  ]
}
```

### Listar Pedidos
```http
GET /api/pedidos
```

### Obter Pedido
```http
GET /api/pedidos/{id}
```

### Atualizar Pedido
```http
PUT /api/pedidos/{id}
Content-Type: application/json

{
  "nomeCliente": "João Silva",
  "emailCliente": "joao@example.com",
  "pago": true,
  "itensPedido": [...]
}
```

### Deletar Pedido
```http
DELETE /api/pedidos/{id}
```

---

## 📦 JSON de Resposta (GET)

```json
{
  "id": 1,
  "nomeCliente": "João Silva",
  "emailCliente": "joao@example.com",
  "pago": true,
  "valorTotal": 200.00,
  "itensPedido": [
    {
      "id": 1,
      "idProduto": 1,
      "nomeProduto": "Produto A",
      "valorUnitario": 100.00,
      "quantidade": 2
    }
  ]
}
```

---

## 🎯 CHECKLIST DOS REQUISITOS ORIGINAIS

### ✅ OBRIGATÓRIO (Eliminatório)

| # | Requisito | Status | Evidência |
|---|-----------|--------|-----------|
| 1 | Projeto compilando com sucesso | ✅ Concluído | Backend .NET 10 e Frontend TypeScript compilam sem erros |
| 2 | Conceitos de DDD, SOLID e Clean Code | ✅ Concluído | Clean Architecture (Domain, Application, Infrastructure, API), entidades ricas, injeção de dependência, SRP |
| 3 | API .NET com CRUD de criar PEDIDO | ✅ Concluído | `POST /api/pedidos`, `GET`, `PUT`, `DELETE` em `PedidosController` |
| 4 | Teste unitário back-end (mínimo GET Pedido) | ✅ Concluído | 32 testes xUnit + Moq (Controller + Service) cobrindo GET, POST, PUT, DELETE |
| 5 | Migration SQL funcionando no projeto | ✅ Concluído | `InitialCreate` migration com EF Core — tabelas Pedidos e ItensPedido |
| 6 | Swagger implementado e funcionando | ✅ Concluído | Disponível em `/swagger` com documentação completa |
| 7 | GET PEDIDO retorna JSON no modelo especificado | ✅ Concluído | `PedidoDTO` retorna: id, nomeCliente, emailCliente, pago, valorTotal (soma qty × valor), itensPedido[] |
| 8 | Projeto publicado em repositório GIT | ✅ Concluído | GitHub: `github.com/diogenesb6/CrudPedidos` |

### 🌟 DESEJÁVEL (Diferencial)

| # | Requisito | Status | Evidência |
|---|-----------|--------|-----------|
| 1 | Arquivo README com informações sobre a aplicação | ✅ Concluído | README completo com arquitetura, setup, endpoints, schema DB |
| 2 | Front-end com integrações e comportamentos básicos | ✅ Concluído | React 19 + TypeScript + Vite — CRUD completo integrado via Fetch API |
| 3 | API e Front-end publicada em Cloud | ⏳ Opcional | Não deployado em Azure/AWS/Heroku |
| 4 | Banco de dados no Docker Compose | ✅ Concluído | `docker-compose.yml` com SQL Server 2022, API .NET 10, Frontend Nginx — healthcheck + migrations automáticas |
| 5 | Design Patterns | ✅ Concluído | Repository Pattern, Service Layer, DTO Pattern, Dependency Injection, Factory (DesignTimeDbContextFactory) |

---

## 📌 PRÓXIMOS PASSOS

### Fase 1: Backend (Prioridade Alta) ✅ CONCLUÍDA
- [x] Criar entidades `Pedido` e `ItemPedido` em `CrudPedidos.Domain`
- [x] Criar DTOs em `CrudPedidos.Application`
- [x] Criar interfaces de repositório em `CrudPedidos.Domain`
- [x] Implementar DbContext em `CrudPedidos.Infrastructure`
- [x] Criar migrations
- [x] Implementar repositórios concretos
- [x] Criar serviços de aplicação
- [x] Criar controllers e endpoints
- [x] Configurar Swagger
- [x] Implementar validações
- [x] Criar testes unitários completos (32 testes - Controller + Service)

### Fase 2: Frontend (Prioridade Alta) ✅ CONCLUÍDA
- [x] Criar estrutura base React 19 com TypeScript e Vite
- [x] Criar serviço Fetch API para comunicação com backend
- [x] Criar componente de listagem (PedidoList)
- [x] Criar formulário de CRUD (PedidoForm) com itens dinâmicos
- [x] Implementar React Router v7 (rotas: /, /pedidos/novo, /pedidos/:id)
- [x] Integrar todos os 5 endpoints CRUD
- [x] Adicionar validações HTML5 nos formulários
- [x] Styling com CSS puro
- [x] Configurar proxy Vite e CORS no backend

### Fase 3: Infraestrutura (Prioridade Média) ✅ CONCLUÍDA
- [x] Criar Dockerfile para API (multi-stage build .NET 10)
- [x] Criar Dockerfile para Frontend (Node 22 + Nginx)
- [x] Criar docker-compose.yml (API + Frontend + SQL Server 2022)
- [x] Configurar nginx.conf (SPA routing + proxy reverso)
- [x] Configurar healthcheck e migration automática
- [x] Criar .dockerignore (API + Frontend)
- [ ] (Opcional) Deploy em cloud

### Fase 4: Qualidade (Prioridade Média)
- [ ] Testes unitários completos (backend)
- [ ] Testes de integração
- [ ] Testes e2e (frontend)
- [ ] Code review e refatoração
- [ ] Documentação final

---

## 👤 Contribuição

As contribuições são bem-vindas! Por favor:

1. Faça um **fork** do projeto
2. Crie uma **branch** para sua feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** suas mudanças (`git commit -m 'Add AmazingFeature'`)
4. **Push** para a branch (`git push origin feature/AmazingFeature`)
5. Abra um **Pull Request**

---

## 📄 Licença

Este projeto está licenciado sob a **MIT License** - veja o arquivo `LICENSE` para detalhes.

---

## 📞 Contato

- **GitHub**: [@diogenesb6](https://github.com/diogenesb6)
- **Repositório**: [CrudPedidos](https://github.com/diogenesb6/CrudPedidos)

---

## 📚 Referências e Recursos

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [xUnit.net](https://xunit.net/)

---

**Última atualização**: 2025  
**Status**: ✅ Fase 1 Backend | ✅ Fase 2 Frontend | ✅ Fase 3 Docker | 🚀 Pronto para Fase 4 (Qualidade)
