# CrudPedidos - Aplicação de Gerenciamento de Pedidos

[![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)]()
[![.NET](https://img.shields.io/badge/.NET-10.0-blue)]()
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
| **Entity Framework Core** | ✅ Concluído | DbContext configurado |
| **Migrations** | ⏳ Próximo | Migration inicial para SQL Server |
| **SQL Server/InMemory** | ✅ Concluído | Suporte para ambos |

#### **6. Documentação API**

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **Swagger** | ✅ Concluído | Habilitado e funcionando em `/swagger` |
| **Documentação** | ✅ Concluído | Endpoints documentados com comentários XML |

#### **7. Testes Unitários**

| Teste | Status | Detalhes |
|-------|--------|---------|
| **Testes GET Pedidos (Controller)** | ✅ Concluído | xUnit + Mocks (11 testes) |
| **Testes CRUD Pedidos (Service)** | ✅ Concluído | xUnit + Mocks (14 testes) |
| **Mocks de Repositórios** | ✅ Concluído | Moq Framework configurado |

---

### 🚀 FRONTEND - React

#### **1. Estrutura do Projeto**

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **Setup React 18+** | ⏳ Não Iniciado | TypeScript, React Router, Axios |
| **Estrutura de Pastas** | ⏳ Não Iniciado | Components, pages, services, hooks |

#### **2. Funcionalidades**

| Funcionalidade | Status | Detalhes |
|----------------|--------|---------|
| **Listagem de Pedidos** | ⏳ Não Iniciado | Tabela/Cards com filtro e ordenação |
| **Visualização Detalhada** | ⏳ Não Iniciado | Página com dados completos |
| **Criar Pedido** | ⏳ Não Iniciado | Formulário dinâmico com itens |
| **Editar Pedido** | ⏳ Não Iniciado | Atualização de dados e itens |
| **Deletar Pedido** | ⏳ Não Iniciado | Com confirmação |
| **Cálculo de ValorTotal** | ⏳ Não Iniciado | Frontend ou exibir da API |

#### **3. Integração com API**

| Integração | Status | Detalhes |
|-----------|--------|---------|
| **Serviço Axios** | ⏳ Não Iniciado | Cliente HTTP customizado |
| **Endpoints CRUD** | ⏳ Não Iniciado | Todos os 5 endpoints integrados |
| **Tratamento de Erros** | ⏳ Não Iniciado | Feedback ao usuário |

---

### 🐳 INFRAESTRUTURA

| Componente | Status | Detalhes |
|-----------|--------|---------|
| **Docker Compose** | ⏳ Não Iniciado | API, Frontend, SQL Server |
| **docker-compose.yml** | ⏳ Não Iniciado | Serviços configurados |
| **Deploy Cloud** | ⏳ Não Iniciado | Instruções Azure/AWS/Heroku (opcional) |

---

## 🏗️ ARQUITETURA DO PROJETO

### Estrutura de Diretórios

```
CrudPedidos/
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
│   ├── Persistence/                 # Migrations
│   └── DependencyInjection/         # Configuração de DI
├── CrudPedidos.API/                 # Camada de Apresentação
│   ├── Controllers/                 # Controllers da API
│   ├── Middlewares/                 # Middlewares customizados
│   └── Program.cs                   # Configuração inicial
├── CrudPedidos.Tests/               # Testes Unitários
│   ├── Controllers/                 # Testes de controller
│   └── Services/                    # Testes de serviço
└── frontend/                        # Frontend React (a criar)
    ├── src/
    │   ├── components/              # Componentes React
    │   ├── pages/                   # Páginas
    │   ├── services/                # Serviço de API (Axios)
    │   ├── hooks/                   # Custom hooks
    │   └── styles/                  # CSS/Styled Components
    └── package.json
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
- **Framework**: React 18+
- **Linguagem**: TypeScript (recomendado)
- **HTTP Client**: Axios
- **Roteamento**: React Router v6+
- **Estilos**: CSS Modules ou Styled Components

### Infraestrutura
- **Containerização**: Docker & Docker Compose
- **Cloud**: Azure/AWS/Heroku (opcional)

---

## 📋 REQUISITOS

- **.NET 10.0** ou superior
- **Node.js 18.0** ou superior
- **npm 9.0** ou superior
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
dotnet ef database update

# Executar a API
dotnet run --project CrudPedidos.API
```

**URL da API**: `https://localhost:7000`  
**Swagger**: `https://localhost:7000/swagger`

### 3. Configurar Frontend

```bash
cd frontend

# Instalar dependências
npm install

# Iniciar servidor de desenvolvimento
npm start
```

**URL do Frontend**: `http://localhost:3000`

### 4. Executar com Docker Compose (quando pronto)

```bash
docker-compose up --build
```

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

## 📝 API - Endpoints

> ⚠️ **Em desenvolvimento** - Endereços e estruturas sujeitos a mudanças

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

## 🎯 PRÓXIMOS PASSOS

### Fase 1: Backend (Prioridade Alta)
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
- [x] Criar testes unitários (GET)

### Fase 2: Frontend (Prioridade Alta)
- [ ] Criar estrutura base React com TypeScript
- [ ] Criar serviço Axios para API
- [ ] Criar componente de listagem
- [ ] Criar formulário de CRUD
- [ ] Implementar React Router
- [ ] Integrar todos os endpoints
- [ ] Adicionar validações
- [ ] Styling (CSS Modules ou Styled Components)

### Fase 3: Infraestrutura (Prioridade Média)
- [ ] Criar Dockerfile para API
- [ ] Criar Dockerfile para Frontend
- [ ] Criar docker-compose.yml
- [ ] Testar containerização local
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

**Última atualização**: 2024  
**Status**: ✅ Fase 1 Backend Concluída | 🚀 Pronto para Fase 2 (Frontend)
