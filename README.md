# CrudOrders - Gerenciamento de Pedidos

[![.NET](https://img.shields.io/badge/.NET-10.0-blue)]()
[![React](https://img.shields.io/badge/React-19-61dafb)]()
[![Vite](https://img.shields.io/badge/Vite-6-646cff)]()
[![Node](https://img.shields.io/badge/Node-22-339933)]()
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ed)]()
[![License](https://img.shields.io/badge/License-MIT-green)]()

## Descricao

Aplicacao completa com backend em **.NET 10 (API REST)** e frontend em **React 19** para gerenciar pedidos com CRUD completo, seguindo boas praticas de arquitetura **(Clean Architecture, DDD, SOLID, Clean Code)** com **32 testes unitarios automatizados**.

---

## Arquitetura do Projeto

### Estrutura de Diretorios

```
CrudOrders/
├── CrudOrders.slnx                   # Solution
├── docker-compose.yml                # Orquestra API + Frontend + SQL Server
├── CrudOrders.Domain/                # Camada de Dominio
│   ├── Entities/                     # Order, OrderItem
│   ├── Interfaces/                   # IOrderRepository
│   └── Resources/                    # Mensagens de validacao
├── CrudOrders.Application/           # Camada de Aplicacao
│   ├── DTOs/                         # CreateOrderDTO, UpdateOrderDTO, OrderDTO, OrderItemDTO
│   ├── Services/                     # OrderService
│   ├── Interfaces/                   # IOrderService
│   └── Mappings/                     # AutoMapper profiles
├── CrudOrders.Infrastructure/        # Camada de Infraestrutura
│   ├── Data/                         # CrudOrdersContext, DesignTimeDbContextFactory
│   ├── Repositories/                 # OrderRepository
│   ├── Migrations/                   # EF Core Migrations
│   └── DependencyInjection/          # ServiceCollectionExtensions
├── CrudOrders.API/                   # Camada de Apresentacao
│   ├── Controllers/                  # OrdersController
│   ├── Resources/                    # Mensagens da API
│   ├── Dockerfile                    # Multi-stage build .NET 10
│   └── Program.cs                    # Configuracao inicial + CORS
├── CrudOrders.Tests/                 # Testes Unitarios
│   ├── Controllers/                  # OrdersControllerTests (16 testes)
│   └── Services/                     # OrderServiceTests (16 testes)
└── frontend/                         # Frontend React 19 + Vite 6
    ├── index.html                    # Entry point (Google Fonts Inter)
    ├── vite.config.js                # Proxy /api -> localhost:5234
    ├── package.json                  # React 19, Vite 6, React Router 7
    └── src/
        ├── main.jsx                  # Entry point React
        ├── App.jsx                   # Rotas e layout (header + navegacao)
        ├── index.css                 # CSS puro (cards, tabelas, badges, forms)
        ├── api/
        │   └── ordersApi.js          # Cliente HTTP (Fetch API)
        └── components/
            ├── OrderList.jsx         # Listagem com toggle Paid/Unpaid e Delete inline
            ├── OrderForm.jsx         # Formulario criar/editar com itens dinamicos
            └── OrderDetails.jsx      # Detalhes do pedido
```

---

## Tecnologias

### Backend
- **.NET 10.0** com ASP.NET Core
- **Entity Framework Core** (SQL Server)
- **AutoMapper** para mapeamento de DTOs
- **Swagger/OpenAPI** para documentacao
- **xUnit + Moq** para testes unitarios

### Frontend
- **React 19** com JSX
- **Vite 6** como build tool
- **React Router v7** para navegacao
- **Fetch API** nativa para chamadas HTTP
- **CSS puro** (sem frameworks CSS)

### Infraestrutura
- **Docker & Docker Compose**
- **Nginx** (frontend em producao)
- **SQL Server 2022**

---

## Requisitos

- **.NET 10.0** ou superior
- **Node.js 22** ou superior
- **SQL Server** (ou InMemory para dev)
- **Docker** (opcional)

---

## Como Executar

### 1. Clonar o Repositorio

```bash
git clone https://github.com/diogenesb6/crudOrders.git
cd crudOrders
```

### 2. Backend (API)

```bash
# Restaurar dependencias
dotnet restore CrudOrders.slnx

# Aplicar migrations (criar banco)
dotnet ef database update --project CrudOrders.Infrastructure --startup-project CrudOrders.API

# Executar a API
dotnet run --project CrudOrders.API
```

| Recurso | URL |
|---------|-----|
| **API** | `http://localhost:5234` |
| **Swagger** | `http://localhost:5234/swagger` |

### 3. Frontend

```bash
cd frontend

# Instalar dependencias
npm install

# Iniciar servidor de desenvolvimento
npm run dev
```

| Recurso | URL |
|---------|-----|
| **Frontend** | `http://localhost:5173` |

> O Vite faz proxy automatico de `/api/*` para `http://localhost:5234`. A API precisa estar rodando.

### 4. Docker Compose (opcional)

```bash
docker-compose up --build
```

| Servico | URL |
|---------|-----|
| **Frontend** | `http://localhost:5173` |
| **API** | `http://localhost:5234` |
| **Swagger** | `http://localhost:5234/swagger` |
| **SQL Server** | `localhost:1433` |

> A API aguarda o SQL Server ficar saudavel (healthcheck) e aplica migrations automaticamente.

---

## Testes Unitarios

Os testes rodam **automaticamente apos cada build**. Tambem podem ser executados manualmente:

```bash
# Todos os testes
dotnet test CrudOrders.slnx

# Apenas o projeto de testes
dotnet test CrudOrders.Tests
```

### Resumo

| Camada | Testes | Descricao |
|--------|--------|-----------|
| **Controller** | 16 | Testa os endpoints HTTP (respostas, status codes, erros) |
| **Service** | 16 | Testa a logica de negocio (validacoes, regras, casos limite) |
| **Total** | **32** | Todos passando |

### Testes do Controller (OrdersControllerTests)

Validam o comportamento dos endpoints HTTP da API, verificando status codes e respostas.

| # | Teste | O que valida |
|---|-------|-------------|
| 1 | `GetAll_ShouldReturnOkWithOrderList` | Listar pedidos retorna 200 OK com a lista |
| 2 | `GetAll_ShouldReturnEmptyList` | Listar pedidos retorna 200 OK com lista vazia quando nao ha pedidos |
| 3 | `GetAll_WhenServiceThrowsException_ShouldReturnInternalServerError` | Erro interno no servico retorna 500 |
| 4 | `GetById_WithValidId_ShouldReturnOkWithOrder` | Buscar pedido por ID valido retorna 200 OK com os dados |
| 5 | `GetById_WithNonExistentId_ShouldReturnNotFound` | Buscar pedido inexistente retorna 404 Not Found |
| 6 | `GetById_WithInvalidId_ShouldReturnBadRequest` | Buscar pedido com ID invalido (0 ou negativo) retorna 400 Bad Request |
| 7 | `Create_WithValidData_ShouldReturnCreatedAtAction` | Criar pedido com dados validos retorna 201 Created |
| 8 | `Create_WithEmptyCustomerName_ShouldReturnBadRequest` | Criar pedido sem nome do cliente retorna 400 |
| 9 | `Create_WithNoItems_ShouldReturnBadRequest` | Criar pedido sem itens retorna 400 |
| 10 | `Update_WithValidData_ShouldReturnOkWithUpdatedOrder` | Atualizar pedido com dados validos retorna 200 OK |
| 11 | `Update_WithNonExistentId_ShouldReturnNotFound` | Atualizar pedido inexistente retorna 404 |
| 12 | `Update_WithEmptyCustomerName_ShouldReturnBadRequest` | Atualizar pedido sem nome do cliente retorna 400 |
| 13 | `Update_WithInvalidId_ShouldReturnBadRequest` | Atualizar pedido com ID invalido retorna 400 |
| 14 | `Update_WhenServiceThrowsException_ShouldReturnInternalServerError` | Erro interno ao atualizar retorna 500 |
| 15 | `Delete_WithValidId_ShouldReturnNoContent` | Deletar pedido existente retorna 204 No Content |
| 16 | `Delete_WithNonExistentId_ShouldReturnNotFound` | Deletar pedido inexistente retorna 404 |

### Testes do Service (OrderServiceTests)

Validam a logica de negocio, validacoes de entrada e interacao com o repositorio.

| # | Teste | O que valida |
|---|-------|-------------|
| 1 | `GetByIdAsync_WithValidId_ShouldReturnOrder` | Buscar por ID valido retorna o pedido mapeado corretamente |
| 2 | `GetByIdAsync_WithInvalidId_ShouldThrowArgumentException` | ID zero ou negativo lanca excecao de argumento |
| 3 | `GetByIdAsync_WithNonExistentId_ShouldReturnNull` | ID inexistente no banco retorna null |
| 4 | `GetAllAsync_ShouldReturnOrderList` | Listar todos retorna a lista mapeada corretamente |
| 5 | `GetAllAsync_WhenEmpty_ShouldReturnEmptyList` | Listar quando nao ha pedidos retorna lista vazia |
| 6 | `CreateAsync_WithValidData_ShouldReturnCreatedOrder` | Criar com dados validos persiste e retorna o pedido com total calculado |
| 7 | `CreateAsync_WithEmptyCustomerName_ShouldThrowArgumentException` | Criar sem nome do cliente lanca excecao |
| 8 | `CreateAsync_WithNoItems_ShouldThrowArgumentException` | Criar sem itens lanca excecao |
| 9 | `CreateAsync_WithNegativeUnitPrice_ShouldThrowArgumentException` | Criar com preco unitario negativo lanca excecao |
| 10 | `UpdateAsync_WithValidData_ShouldReturnUpdatedOrder` | Atualizar com dados validos persiste e retorna o pedido atualizado |
| 11 | `UpdateAsync_WithInvalidId_ShouldThrowArgumentException` | Atualizar com ID invalido lanca excecao |
| 12 | `UpdateAsync_WithNonExistentId_ShouldThrowInvalidOperationException` | Atualizar pedido inexistente lanca excecao de operacao invalida |
| 13 | `UpdateAsync_WithEmptyCustomerName_ShouldThrowArgumentException` | Atualizar sem nome do cliente lanca excecao |
| 14 | `UpdateAsync_WithEmptyCustomerEmail_ShouldThrowArgumentException` | Atualizar sem email do cliente lanca excecao |
| 15 | `DeleteAsync_WithValidId_ShouldReturnTrue` | Deletar pedido existente retorna true |
| 16 | `DeleteAsync_WithNonExistentId_ShouldThrowInvalidOperationException` | Deletar pedido inexistente lanca excecao |

---

## Banco de Dados

### Tabela: Orders

| Coluna | Tipo | Restricoes |
|--------|------|-----------|
| Id | int | PK, Auto-increment |
| CustomerName | nvarchar(255) | NOT NULL |
| CustomerEmail | nvarchar(255) | NOT NULL |
| Paid | bit | NOT NULL, DEFAULT: false |
| TotalAmount | decimal(18,2) | NOT NULL |
| CreatedAt | datetime2 | NOT NULL, DEFAULT: GETUTCDATE() |
| UpdatedAt | datetime2 | NULL |

### Tabela: OrderItems

| Coluna | Tipo | Restricoes |
|--------|------|-----------|
| Id | int | PK, Auto-increment |
| ProductId | int | NOT NULL |
| ProductName | nvarchar(255) | NOT NULL |
| UnitPrice | decimal(18,2) | NOT NULL |
| Quantity | int | NOT NULL |
| OrderId | int | NOT NULL, FK -> Orders.Id (CASCADE) |

**Relacionamento**: Um pedido pode ter multiplos itens. Ao deletar um pedido, todos os itens sao removidos automaticamente.

### Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project CrudOrders.Infrastructure --startup-project CrudOrders.API

# Aplicar migrations
dotnet ef database update --project CrudOrders.Infrastructure --startup-project CrudOrders.API

# Remover ultima migration
dotnet ef migrations remove --project CrudOrders.Infrastructure --startup-project CrudOrders.API
```

---

## API - Endpoints

### Criar Pedido
```http
POST /api/orders
Content-Type: application/json

{
  "nomeCliente": "John Doe",
  "emailCliente": "john@example.com",
  "pago": false,
  "itensPedido": [
    {
      "idProduto": 1,
      "nomeProduto": "Widget",
      "valorUnitario": 9.99,
      "quantidade": 2
    }
  ]
}
```

### Listar Pedidos
```http
GET /api/orders
```

### Obter Pedido
```http
GET /api/orders/{id}
```

### Atualizar Pedido
```http
PUT /api/orders/{id}
Content-Type: application/json

{
  "nomeCliente": "John Doe",
  "emailCliente": "john@example.com",
  "pago": true,
  "itensPedido": [...]
}
```

### Deletar Pedido
```http
DELETE /api/orders/{id}
```

### Resposta (GET)

```json
{
  "id": 1,
  "nomeCliente": "John Doe",
  "emailCliente": "john@example.com",
  "pago": true,
  "valorTotal": 19.98,
  "itensPedido": [
    {
      "id": 1,
      "idProduto": 1,
      "nomeProduto": "Widget",
      "valorUnitario": 9.99,
      "quantidade": 2
    }
  ]
}
```

---

## Funcionalidades do Frontend

| Funcionalidade | Descricao |
|----------------|-----------|
| **Listagem de Pedidos** | Tabela com dados, badges de status, acoes inline |
| **Toggle Paid/Unpaid** | Clique no badge para alternar o status de pagamento |
| **Criar Pedido** | Formulario com adicao/remocao dinamica de itens |
| **Editar Pedido** | Carrega dados existentes e atualiza incluindo itens |
| **Deletar Pedido** | Botao inline na listagem com confirmacao |
| **Detalhes do Pedido** | Visualizacao completa com lista de itens e subtotais |

---

## Design Patterns

| Pattern | Onde |
|---------|-----|
| **Clean Architecture** | Separacao em Domain, Application, Infrastructure, API |
| **Repository Pattern** | `IOrderRepository` / `OrderRepository` |
| **Service Layer** | `IOrderService` / `OrderService` |
| **DTO Pattern** | `CreateOrderDTO`, `UpdateOrderDTO`, `OrderDTO` |
| **Dependency Injection** | `ServiceCollectionExtensions` |
| **Factory** | `DesignTimeDbContextFactory` |

---

## Contribuicao

1. Faca um **fork** do projeto
2. Crie uma **branch** para sua feature (`git checkout -b feature/NovaFeature`)
3. **Commit** suas mudancas (`git commit -m 'Add NovaFeature'`)
4. **Push** para a branch (`git push origin feature/NovaFeature`)
5. Abra um **Pull Request**

---

## Licenca

Este projeto esta licenciado sob a **MIT License**.

---

## Contato

- **GitHub**: [@diogenesb6](https://github.com/diogenesb6)
- **Repositorio**: [crudOrders](https://github.com/diogenesb6/crudOrders)

---

## Referencias

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [xUnit.net](https://xunit.net/)
