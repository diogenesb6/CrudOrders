using AutoMapper;
using CrudPedidos.Application.DTOs;
using CrudPedidos.Application.Mappings;
using CrudPedidos.Application.Services;
using CrudPedidos.Domain.Entities;
using CrudPedidos.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CrudPedidos.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<OrderProfile>());
        var serviceProvider = services.BuildServiceProvider();
        _mapper = serviceProvider.GetRequiredService<IMapper>();

        _service = new OrderService(_repositoryMock.Object, _mapper);
    }

    #region GetById

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnOrder()
    {
        // Arrange
        int id = 1;
        var expectedOrder = new Order(
            "João Silva",
            "joao@example.com",
            new List<OrderItem>
            {
                new OrderItem(1, "Produto A", 100.00m, 2)
            }
        )
        {
            Id = id
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("João Silva", result.CustomerName);
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldThrowArgumentException()
    {
        // Arrange
        int id = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetByIdAsync(id));
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        int id = 999;
        _repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetAll

    [Fact]
    public async Task GetAllAsync_ShouldReturnOrderList()
    {
        // Arrange
        var expectedOrders = new List<Order>
        {
            new Order(
                "João Silva",
                "joao@example.com",
                new List<OrderItem>
                {
                    new OrderItem(1, "Produto A", 100.00m, 2)
                }
            ) { Id = 1 }
        };

        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedOrders);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var orders = result.ToList();
        Assert.Single(orders);
        Assert.Equal("João Silva", orders[0].CustomerName);
    }

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Order>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region Create

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnCreatedOrder()
    {
        // Arrange
        var createDto = new CreateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>
            {
                new CreateOrderItemDTO
                {
                    ProductId = 1,
                    ProductName = "Produto A",
                    UnitPrice = 100.00m,
                    Quantity = 2
                }
            }
        };

        var createdOrder = new Order(
            createDto.CustomerName,
            createDto.CustomerEmail,
            createDto.OrderItems.Select(i => new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity)).ToList()
        )
        {
            Id = 1
        };

        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync(createdOrder);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("João Silva", result.CustomerName);
        Assert.Equal(200.00m, result.TotalAmount);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyCustomerName_ShouldThrowArgumentException()
    {
        // Arrange
        var createDto = new CreateOrderDTO
        {
            CustomerName = "",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_WithNoItems_ShouldThrowArgumentException()
    {
        // Arrange
        var createDto = new CreateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_WithNegativeUnitPrice_ShouldThrowArgumentException()
    {
        // Arrange
        var createDto = new CreateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>
            {
                new CreateOrderItemDTO
                {
                    ProductId = 1,
                    ProductName = "Produto A",
                    UnitPrice = -100.00m,
                    Quantity = 2
                }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
    }

    #endregion

    #region Update

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldReturnUpdatedOrder()
    {
        // Arrange
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "João Silva Atualizado",
            CustomerEmail = "joao.atualizado@example.com",
            Paid = true,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        var existingOrder = new Order(
            "João Silva",
            "joao@example.com",
            new List<OrderItem>
            {
                new OrderItem(1, "Produto A", 100.00m, 2)
            }
        )
        {
            Id = id
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existingOrder);

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order p) => p);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("João Silva Atualizado", result.CustomerName);
        Assert.Equal("joao.atualizado@example.com", result.CustomerEmail);
        Assert.True(result.Paid);
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldThrowArgumentException()
    {
        // Arrange
        int id = 0;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(id, updateDto));
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        int id = 999;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Order?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(id, updateDto));
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyCustomerName_ShouldThrowArgumentException()
    {
        // Arrange
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(id, updateDto));
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyCustomerEmail_ShouldThrowArgumentException()
    {
        // Arrange
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(id, updateDto));
    }

    #endregion

    #region Delete

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        int id = 1;
        _repositoryMock.Setup(r => r.ExistsAsync(id))
            .ReturnsAsync(true);
        _repositoryMock.Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        int id = 999;
        _repositoryMock.Setup(r => r.ExistsAsync(id))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteAsync(id));
    }

    #endregion
}
