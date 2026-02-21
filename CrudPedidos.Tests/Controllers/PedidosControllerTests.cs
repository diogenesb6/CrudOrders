using CrudPedidos.Application.DTOs;
using CrudPedidos.Application.Interfaces;
using CrudPedidos.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CrudPedidos.Tests;

public class PedidosControllerTests
{
    private readonly Mock<IPedidoService> _pedidoServiceMock;
    private readonly Mock<ILogger<PedidosController>> _loggerMock;
    private readonly PedidosController _controller;

    public PedidosControllerTests()
    {
        _pedidoServiceMock = new Mock<IPedidoService>();
        _loggerMock = new Mock<ILogger<PedidosController>>();
        _controller = new PedidosController(_pedidoServiceMock.Object, _loggerMock.Object);
    }

    #region ObterTodos

    [Fact]
    public async Task ObterTodos_DeveRetornarOkComListaPedidos()
    {
        // Arrange
        var pedidosEsperados = new List<PedidoDTO>
        {
            new PedidoDTO
            {
                Id = 1,
                NomeCliente = "João Silva",
                EmailCliente = "joao@example.com",
                Pago = false,
                ValorTotal = 200.00m,
                ItensPedido = new List<ItemPedidoDTO>
                {
                    new ItemPedidoDTO
                    {
                        Id = 1,
                        IdProduto = 1,
                        NomeProduto = "Produto A",
                        ValorUnitario = 100.00m,
                        Quantidade = 2
                    }
                }
            }
        };

        _pedidoServiceMock.Setup(s => s.ObterTodosAsync())
            .ReturnsAsync(pedidosEsperados);

        // Act
        var resultado = await _controller.ObterTodos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        Assert.NotNull(okResult.Value);
        var pedidos = Assert.IsAssignableFrom<IEnumerable<PedidoDTO>>(okResult.Value);
        Assert.Single(pedidos);
        _pedidoServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarListaVazia()
    {
        // Arrange
        var pedidosEsperados = new List<PedidoDTO>();

        _pedidoServiceMock.Setup(s => s.ObterTodosAsync())
            .ReturnsAsync(pedidosEsperados);

        // Act
        var resultado = await _controller.ObterTodos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var pedidos = Assert.IsAssignableFrom<IEnumerable<PedidoDTO>>(okResult.Value);
        Assert.Empty(pedidos);
    }

    [Fact]
    public async Task ObterTodos_QuandoServicoLancaExcecao_DeveRetornarInternalServerError()
    {
        // Arrange
        _pedidoServiceMock.Setup(s => s.ObterTodosAsync())
            .ThrowsAsync(new Exception("Erro na base de dados"));

        // Act
        var resultado = await _controller.ObterTodos();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(resultado.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    #endregion

    #region ObterPorId

    [Fact]
    public async Task ObterPorId_ComIdValido_DeveRetornarOkComPedido()
    {
        // Arrange
        int id = 1;
        var pedidoEsperado = new PedidoDTO
        {
            Id = id,
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ValorTotal = 200.00m,
            ItensPedido = new List<ItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.ObterPorIdAsync(id))
            .ReturnsAsync(pedidoEsperado);

        // Act
        var resultado = await _controller.ObterPorId(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var pedido = Assert.IsType<PedidoDTO>(okResult.Value);
        Assert.Equal(id, pedido.Id);
        Assert.Equal("João Silva", pedido.NomeCliente);
    }

    [Fact]
    public async Task ObterPorId_ComIdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        int id = 999;
        _pedidoServiceMock.Setup(s => s.ObterPorIdAsync(id))
            .ReturnsAsync((PedidoDTO?)null);

        // Act
        var resultado = await _controller.ObterPorId(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task ObterPorId_ComIdInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        int id = 0;
        _pedidoServiceMock.Setup(s => s.ObterPorIdAsync(id))
            .ThrowsAsync(new ArgumentException("Id deve ser maior que zero"));

        // Act
        var resultado = await _controller.ObterPorId(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    #endregion

    #region Criar

    [Fact]
    public async Task Criar_ComDadosValidos_DeveRetornarCreatedAtAction()
    {
        // Arrange
        var criarDto = new CriarPedidoDTO
        {
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>
            {
                new CriarItemPedidoDTO
                {
                    IdProduto = 1,
                    NomeProduto = "Produto A",
                    ValorUnitario = 100.00m,
                    Quantidade = 2
                }
            }
        };

        var pedidoCriado = new PedidoDTO
        {
            Id = 1,
            NomeCliente = criarDto.NomeCliente,
            EmailCliente = criarDto.EmailCliente,
            Pago = criarDto.Pago,
            ValorTotal = 200.00m,
            ItensPedido = new List<ItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.CriarAsync(criarDto))
            .ReturnsAsync(pedidoCriado);

        // Act
        var resultado = await _controller.Criar(criarDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
        Assert.Equal(nameof(PedidosController.ObterPorId), createdResult.ActionName);
        Assert.Equal(pedidoCriado.Id, ((PedidoDTO)createdResult.Value!).Id);
    }

    [Fact]
    public async Task Criar_ComNomeClienteVazio_DeveRetornarBadRequest()
    {
        // Arrange
        var criarDto = new CriarPedidoDTO
        {
            NomeCliente = "",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.CriarAsync(criarDto))
            .ThrowsAsync(new ArgumentException("Nome do cliente é obrigatório"));

        // Act
        var resultado = await _controller.Criar(criarDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Criar_SemItens_DeveRetornarBadRequest()
    {
        // Arrange
        var criarDto = new CriarPedidoDTO
        {
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.CriarAsync(criarDto))
            .ThrowsAsync(new ArgumentException("Pedido deve conter pelo menos um item"));

        // Act
        var resultado = await _controller.Criar(criarDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    #endregion

    #region Atualizar

    [Fact]
    public async Task Atualizar_ComDadosValidos_DeveRetornarOkComPedidoAtualizado()
    {
        // Arrange
        int id = 1;
        var atualizarDto = new AtualizarPedidoDTO
        {
            NomeCliente = "João Silva Atualizado",
            EmailCliente = "joao.atualizado@example.com",
            Pago = true,
            ItensPedido = new List<CriarItemPedidoDTO>
            {
                new CriarItemPedidoDTO
                {
                    IdProduto = 1,
                    NomeProduto = "Produto A",
                    ValorUnitario = 150.00m,
                    Quantidade = 3
                }
            }
        };

        var pedidoAtualizado = new PedidoDTO
        {
            Id = id,
            NomeCliente = atualizarDto.NomeCliente,
            EmailCliente = atualizarDto.EmailCliente,
            Pago = atualizarDto.Pago,
            ValorTotal = 450.00m,
            ItensPedido = new List<ItemPedidoDTO>
            {
                new ItemPedidoDTO
                {
                    Id = 1,
                    IdProduto = 1,
                    NomeProduto = "Produto A",
                    ValorUnitario = 150.00m,
                    Quantidade = 3
                }
            }
        };

        _pedidoServiceMock.Setup(s => s.AtualizarAsync(id, atualizarDto))
            .ReturnsAsync(pedidoAtualizado);

        // Act
        var resultado = await _controller.Atualizar(id, atualizarDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        var pedido = Assert.IsType<PedidoDTO>(okResult.Value);
        Assert.Equal(id, pedido.Id);
        Assert.Equal("João Silva Atualizado", pedido.NomeCliente);
        Assert.True(pedido.Pago);
        _pedidoServiceMock.Verify(s => s.AtualizarAsync(id, atualizarDto), Times.Once);
    }

    [Fact]
    public async Task Atualizar_ComIdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        int id = 999;
        var atualizarDto = new AtualizarPedidoDTO
        {
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.AtualizarAsync(id, atualizarDto))
            .ThrowsAsync(new InvalidOperationException($"Pedido com id {id} não encontrado"));

        // Act
        var resultado = await _controller.Atualizar(id, atualizarDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Atualizar_ComNomeClienteVazio_DeveRetornarBadRequest()
    {
        // Arrange
        int id = 1;
        var atualizarDto = new AtualizarPedidoDTO
        {
            NomeCliente = "",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.AtualizarAsync(id, atualizarDto))
            .ThrowsAsync(new ArgumentException("Nome do cliente é obrigatório"));

        // Act
        var resultado = await _controller.Atualizar(id, atualizarDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Atualizar_ComIdInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        int id = 0;
        var atualizarDto = new AtualizarPedidoDTO
        {
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.AtualizarAsync(id, atualizarDto))
            .ThrowsAsync(new ArgumentException("Id deve ser maior que zero"));

        // Act
        var resultado = await _controller.Atualizar(id, atualizarDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado.Result);
    }

    [Fact]
    public async Task Atualizar_QuandoServicoLancaExcecao_DeveRetornarInternalServerError()
    {
        // Arrange
        int id = 1;
        var atualizarDto = new AtualizarPedidoDTO
        {
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        _pedidoServiceMock.Setup(s => s.AtualizarAsync(id, atualizarDto))
            .ThrowsAsync(new Exception("Erro inesperado na base de dados"));

        // Act
        var resultado = await _controller.Atualizar(id, atualizarDto);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(resultado.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    #endregion

    #region Deletar

    [Fact]
    public async Task Deletar_ComIdValido_DeveRetornarNoContent()
    {
        // Arrange
        int id = 1;
        _pedidoServiceMock.Setup(s => s.DeletarAsync(id))
            .ReturnsAsync(true);

        // Act
        var resultado = await _controller.Deletar(id);

        // Assert
        Assert.IsType<NoContentResult>(resultado);
        _pedidoServiceMock.Verify(s => s.DeletarAsync(id), Times.Once);
    }

    [Fact]
    public async Task Deletar_ComIdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        int id = 999;
        _pedidoServiceMock.Setup(s => s.DeletarAsync(id))
            .ThrowsAsync(new InvalidOperationException($"Pedido com id {id} não encontrado"));

        // Act
        var resultado = await _controller.Deletar(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado);
    }

    #endregion
}
