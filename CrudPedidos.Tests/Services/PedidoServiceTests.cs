using AutoMapper;
using CrudPedidos.Application.DTOs;
using CrudPedidos.Application.Mappings;
using CrudPedidos.Application.Services;
using CrudPedidos.Domain.Entities;
using CrudPedidos.Domain.Interfaces;
using Moq;

namespace CrudPedidos.Tests;

public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        _repositoryMock = new Mock<IPedidoRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PedidoProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new PedidoService(_repositoryMock.Object, _mapper);
    }

    #region ObterPorId

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarPedido()
    {
        // Arrange
        int id = 1;
        var pedidoEsperado = new Pedido(
            "João Silva",
            "joao@example.com",
            new List<ItemPedido>
            {
                new ItemPedido(1, "Produto A", 100.00m, 2)
            }
        )
        {
            Id = id
        };

        _repositoryMock.Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync(pedidoEsperado);

        // Act
        var resultado = await _service.ObterPorIdAsync(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
        Assert.Equal("João Silva", resultado.NomeCliente);
        _repositoryMock.Verify(r => r.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInvalido_DeveLancarArgumentException()
    {
        // Arrange
        int id = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ObterPorIdAsync(id));
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInexistente_DeveRetornarNull()
    {
        // Arrange
        int id = 999;
        _repositoryMock.Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync((Pedido?)null);

        // Act
        var resultado = await _service.ObterPorIdAsync(id);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region ObterTodos

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaPedidos()
    {
        // Arrange
        var pedidosEsperados = new List<Pedido>
        {
            new Pedido(
                "João Silva",
                "joao@example.com",
                new List<ItemPedido>
                {
                    new ItemPedido(1, "Produto A", 100.00m, 2)
                }
            ) { Id = 1 }
        };

        _repositoryMock.Setup(r => r.ObterTodosAsync())
            .ReturnsAsync(pedidosEsperados);

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        var pedidos = resultado.ToList();
        Assert.Single(pedidos);
        Assert.Equal("João Silva", pedidos[0].NomeCliente);
    }

    [Fact]
    public async Task ObterTodosAsync_QuandoVazio_DeveRetornarListaVazia()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ObterTodosAsync())
            .ReturnsAsync(new List<Pedido>());

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    #endregion

    #region Criar

    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveRetornarPedidoCriado()
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

        var pedidoCriado = new Pedido(
            criarDto.NomeCliente,
            criarDto.EmailCliente,
            criarDto.ItensPedido.Select(i => new ItemPedido(i.IdProduto, i.NomeProduto, i.ValorUnitario, i.Quantidade)).ToList()
        )
        {
            Id = 1
        };

        _repositoryMock.Setup(r => r.CriarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync(pedidoCriado);

        // Act
        var resultado = await _service.CriarAsync(criarDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("João Silva", resultado.NomeCliente);
        Assert.Equal(200.00m, resultado.ValorTotal);
    }

    [Fact]
    public async Task CriarAsync_ComNomeClienteVazio_DeveLancarArgumentException()
    {
        // Arrange
        var criarDto = new CriarPedidoDTO
        {
            NomeCliente = "",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(criarDto));
    }

    [Fact]
    public async Task CriarAsync_SemItens_DeveLancarArgumentException()
    {
        // Arrange
        var criarDto = new CriarPedidoDTO
        {
            NomeCliente = "João Silva",
            EmailCliente = "joao@example.com",
            Pago = false,
            ItensPedido = new List<CriarItemPedidoDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(criarDto));
    }

    [Fact]
    public async Task CriarAsync_ComValorUnitarioNegativo_DeveLancarArgumentException()
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
                    ValorUnitario = -100.00m,
                    Quantidade = 2
                }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(criarDto));
    }

    #endregion

    #region Deletar

    [Fact]
    public async Task DeletarAsync_ComIdValido_DeveRetornarTrue()
    {
        // Arrange
        int id = 1;
        _repositoryMock.Setup(r => r.ExisteAsync(id))
            .ReturnsAsync(true);
        _repositoryMock.Setup(r => r.DeletarAsync(id))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.DeletarAsync(id);

        // Assert
        Assert.True(resultado);
        _repositoryMock.Verify(r => r.DeletarAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeletarAsync_ComIdInexistente_DeveLancarInvalidOperationException()
    {
        // Arrange
        int id = 999;
        _repositoryMock.Setup(r => r.ExisteAsync(id))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeletarAsync(id));
    }

    #endregion
}
