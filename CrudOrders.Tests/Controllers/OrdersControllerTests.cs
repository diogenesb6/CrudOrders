using CrudOrders.Application.DTOs;
using CrudOrders.Application.Interfaces;
using CrudOrders.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CrudOrders.Tests;

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

    [Fact]
    public async Task GetAll_ShouldReturnOkWithOrderList()
    {
        var expectedOrders = new List<OrderDTO>
        {
            new OrderDTO
            {
                Id = 1,
                CustomerName = "John Silva",
                CustomerEmail = "john@example.com",
                Paid = false,
                TotalAmount = 200.00m,
                OrderItems = new List<OrderItemDTO>
                {
                    new OrderItemDTO
                    {
                        Id = 1,
                        ProductId = 1,
                        ProductName = "Product A",
                        UnitPrice = 100.00m,
                        Quantity = 2
                    }
                }
            }
        };

        _orderServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(expectedOrders);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        var orders = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(okResult.Value);
        Assert.Single(orders);
        _orderServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList()
    {
        var expectedOrders = new List<OrderDTO>();

        _orderServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(expectedOrders);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var orders = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(okResult.Value);
        Assert.Empty(orders);
    }

    [Fact]
    public async Task GetAll_WhenServiceThrowsException_ShouldReturnInternalServerError()
    {
        _orderServiceMock.Setup(s => s.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));

        var result = await _controller.GetAll();

        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnOkWithOrder()
    {
        int id = 1;
        var expectedOrder = new OrderDTO
        {
            Id = id,
            CustomerName = "John Silva",
            CustomerEmail = "john@example.com",
            Paid = false,
            TotalAmount = 200.00m,
            OrderItems = new List<OrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(expectedOrder);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var order = Assert.IsType<OrderDTO>(okResult.Value);
        Assert.Equal(id, order.Id);
        Assert.Equal("John Silva", order.CustomerName);
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ShouldReturnNotFound()
    {
        int id = 999;
        _orderServiceMock.Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((OrderDTO?)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnBadRequest()
    {
        int id = 0;
        _orderServiceMock.Setup(s => s.GetByIdAsync(id))
            .ThrowsAsync(new ArgumentException("Id must be greater than zero"));

        var result = await _controller.GetById(id);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturnCreatedAtAction()
    {
        var createDto = new CreateOrderDTO
        {
            CustomerName = "John Silva",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>
            {
                new CreateOrderItemDTO
                {
                    ProductId = 1,
                    ProductName = "Product A",
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

        var result = await _controller.Create(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(OrdersController.GetById), createdResult.ActionName);
        Assert.Equal(createdOrder.Id, ((OrderDTO)createdResult.Value!).Id);
    }

    [Fact]
    public async Task Create_WithEmptyCustomerName_ShouldReturnBadRequest()
    {
        var createDto = new CreateOrderDTO
        {
            CustomerName = "",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.CreateAsync(createDto))
            .ThrowsAsync(new ArgumentException("Customer name is required"));

        var result = await _controller.Create(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_WithNoItems_ShouldReturnBadRequest()
    {
        var createDto = new CreateOrderDTO
        {
            CustomerName = "John Silva",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.CreateAsync(createDto))
            .ThrowsAsync(new ArgumentException("Order must contain at least one item"));

        var result = await _controller.Create(createDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WithValidData_ShouldReturnOkWithUpdatedOrder()
    {
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "John Silva Updated",
            CustomerEmail = "john.updated@example.com",
            Paid = true,
            OrderItems = new List<CreateOrderItemDTO>
            {
                new CreateOrderItemDTO
                {
                    ProductId = 1,
                    ProductName = "Product A",
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
                    ProductName = "Product A",
                    UnitPrice = 150.00m,
                    Quantity = 3
                }
            }
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ReturnsAsync(updatedOrder);

        var result = await _controller.Update(id, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var order = Assert.IsType<OrderDTO>(okResult.Value);
        Assert.Equal(id, order.Id);
        Assert.Equal("John Silva Updated", order.CustomerName);
        Assert.True(order.Paid);
        _orderServiceMock.Verify(s => s.UpdateAsync(id, updateDto), Times.Once);
    }

    [Fact]
    public async Task Update_WithNonExistentId_ShouldReturnNotFound()
    {
        int id = 999;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "John Silva",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new InvalidOperationException($"Order with id {id} not found"));

        var result = await _controller.Update(id, updateDto);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WithEmptyCustomerName_ShouldReturnBadRequest()
    {
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new ArgumentException("Customer name is required"));

        var result = await _controller.Update(id, updateDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WithInvalidId_ShouldReturnBadRequest()
    {
        int id = 0;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "John Silva",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new ArgumentException("Id must be greater than zero"));

        var result = await _controller.Update(id, updateDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_WhenServiceThrowsException_ShouldReturnInternalServerError()
    {
        int id = 1;
        var updateDto = new UpdateOrderDTO
        {
            CustomerName = "John Silva",
            CustomerEmail = "john@example.com",
            Paid = false,
            OrderItems = new List<CreateOrderItemDTO>()
        };

        _orderServiceMock.Setup(s => s.UpdateAsync(id, updateDto))
            .ThrowsAsync(new Exception("Unexpected database error"));

        var result = await _controller.Update(id, updateDto);

        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        int id = 1;
        _orderServiceMock.Setup(s => s.DeleteAsync(id))
            .ReturnsAsync(true);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
        _orderServiceMock.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnNotFound()
    {
        int id = 999;
        _orderServiceMock.Setup(s => s.DeleteAsync(id))
            .ThrowsAsync(new InvalidOperationException($"Order with id {id} not found"));

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundObjectResult>(result);
    }

}
