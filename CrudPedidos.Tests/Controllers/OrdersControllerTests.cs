using CrudPedidos.Application.DTOs;
using CrudPedidos.Application.Interfaces;
using CrudPedidos.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CrudPedidos.Tests;

public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly Mock<ILogger<OrdersController>> _loggerMock;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _loggerMock = new Mock<ILogger<OrdersController>>();
        _controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);
    }

    #region GetAll

    [Fact]
    public async Task GetAll_ShouldReturnOkWithOrderList()
    {
        // Arrange
        var expectedOrders = new List<OrderDTO>
        {
            new OrderDTO
            {
                Id = 1,
                CustomerName = "João Silva",
                CustomerEmail = "joao@example.com",
                Paid = false,
                TotalAmount = 200.00m,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        Id = 1,
                        ProductId = 1,
                        ProductName = "Produto A",
                        UnitPrice = 100.00m,
                        Quantity = 2
                    }
                }
            }
        };

        _orderServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(expectedOrders);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        var orders = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(okResult.Value);
        Assert.Single(orders);
        _orderServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList()
    {
        // Arrange
        var expectedOrders = new List<OrderDTO>();

        _orderServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(expectedOrders);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var orders = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(okResult.Value);
        Assert.Empty(orders);
    }

    [Fact]
    public async Task GetAll_WhenServiceThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        _orderServiceMock.Setup(s => s.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnOkWithOrder()
    {
        // Arrange
        int id = 1;
        var expectedOrder = new OrderDTO
        {
            Id = id,
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            TotalAmount = 200.00m,
            OrderItems = new List<OrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var order = Assert.IsType<OrderDTO>(okResult.Value);
        Assert.Equal(id, order.Id);
        Assert.Equal("João Silva", order.CustomerName);
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        int id = 999;
        _orderServiceMock.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((OrderDTO?)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        int id = 0;
        _orderServiceMock.Setup(s => s.GetByIdAsync(id))
            .ThrowsAsync(new ArgumentException("Id must be greater than zero"));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_WithValidData_ShouldReturnCreatedAtAction()
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

        var createdOrder = new OrderDTO
        {
            Id = 1,
            CustomerName = createDto.CustomerName,
            CustomerEmail = createDto.CustomerEmail,
            Paid = createDto.Paid,
            TotalAmount = 200.00m,
            OrderItems = new List<OrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.CreateAsync(createDto))
            .ReturnsAsync(createdOrder);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(OrdersController.GetById), createdResult.ActionName);
        Assert.Equal(createdOrder.Id, ((OrderDTO)createdResult.Value!).Id);
    }

    [Fact]
    public async Task Create_WithEmptyCustomerName_ShouldReturnBadRequest()
    {
        // Arrange
        var createDto = new CreateOrderDTO
        {
            CustomerName = "",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.CreateAsync(createDto))
            .ThrowsAsync(new ArgumentException("Customer name is required"));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_WithNoItems_ShouldReturnBadRequest()
    {
        // Arrange
        var createDto = new CreateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.CreateAsync(createDto))
            .ThrowsAsync(new ArgumentException("Order must contain at least one item"));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_WithValidData_ShouldReturnOkWithUpdatedOrder()
    {
        // Arrange
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "João Silva Atualizado",
            CustomerEmail = "joao.atualizado@example.com",
            Paid = true,
            OrderItems = new List<CreateOrderItemDTO>
            {
                new CreateOrderItemDTO
                {
                    ProductId = 1,
                    ProductName = "Produto A",
                    UnitPrice = 150.00m,
                    Quantity = 3
                }
            }
        };

        var updatedOrder = new OrderDTO
        {
            Id = id,
            CustomerName = updateDto.CustomerName,
            CustomerEmail = updateDto.CustomerEmail,
            Paid = updateDto.Paid,
            TotalAmount = 450.00m,
            OrderItems = new List<OrderItemDTO>
            {
                new OrderItemDTO
                {
                    Id = 1,
                    ProductId = 1,
                    ProductName = "Produto A",
                    UnitPrice = 150.00m,
                    Quantity = 3
                }
            }
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ReturnsAsync(updatedOrder);

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var order = Assert.IsType<OrderDTO>(okResult.Value);
        Assert.Equal(id, order.Id);
        Assert.Equal("João Silva Atualizado", order.CustomerName);
        Assert.True(order.Paid);
        _orderServiceMock.Verify(s => s.UpdateAsync(id, updateDto), Times.Once);
    }

    [Fact]
    public async Task Update_WithNonExistentId_ShouldReturnNotFound()
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

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new InvalidOperationException($"Order with id {id} not found"));

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WithEmptyCustomerName_ShouldReturnBadRequest()
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

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new ArgumentException("Customer name is required"));

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WithInvalidId_ShouldReturnBadRequest()
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

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new ArgumentException("Id must be greater than zero"));

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WhenServiceThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "João Silva",
            CustomerEmail = "joao@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new Exception("Unexpected database error"));

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        int id = 1;
        _orderServiceMock.Setup(s => s.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _orderServiceMock.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        int id = 999;
        _orderServiceMock.Setup(s => s.DeleteAsync(id))
            .ThrowsAsync(new InvalidOperationException($"Order with id {id} not found"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    #endregion
}
