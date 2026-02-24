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
git clone https://github.com/diogenesb6/CrudPedidos.git
cd CrudPedidos
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

### Cobertura

| Camada | Testes | Descricao |
|--------|--------|-----------|
| **Controller** | 16 | GET, POST, PUT, DELETE + cenarios de erro |
| **Service** | 16 | Logica de negocio, validacoes, casos limite |
| **Total** | **32** | Todos passando |

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
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "paid": false,
  "orderItems": [
    {
      "productId": 1,
      "productName": "Widget",
      "unitPrice": 9.99,
      "quantity": 2
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
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "paid": true,
  "orderItems": [...]
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
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "paid": true,
  "totalAmount": 19.98,
  "orderItems": [
    {
      "id": 1,
      "productId": 1,
      "productName": "Widget",
      "unitPrice": 9.99,
      "quantity": 2
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
- **Repositorio**: [CrudOrders](https://github.com/diogenesb6/CrudPedidos)

---

## Referencias

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [xUnit.net](https://xunit.net/)
