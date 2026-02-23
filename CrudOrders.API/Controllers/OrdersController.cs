using CrudOrders.Application.DTOs;
using CrudOrders.Application.Interfaces;
using CrudOrders.API.Resources;
using Microsoft.AspNetCore.Mvc;

namespace CrudOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
    {
        try
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ExcMSG1);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = Messages.ExcMSG1, error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDTO>> GetById(int id)
    {
        try
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = string.Format(Messages.InfMSG1, id) });

            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, Messages.InfMSG2);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ExcMSG2);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = Messages.ExcMSG2, error = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDTO>> Create([FromBody] CreateOrderDTO dto)
    {
        try
        {
            var createdOrder = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, Messages.InfMSG3);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ExcMSG3);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = Messages.ExcMSG3, error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDTO>> Update(int id, [FromBody] UpdateOrderDTO dto)
    {
        try
        {
            var updatedOrder = await _orderService.UpdateAsync(id, dto);
            return Ok(updatedOrder);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, Messages.InfMSG4);
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, string.Format(Messages.InfMSG5, id));
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ExcMSG4);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = Messages.ExcMSG4, error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = string.Format(Messages.InfMSG1, id) });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, Messages.InfMSG6);
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, string.Format(Messages.InfMSG5, id));
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.ExcMSG5);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = Messages.ExcMSG5, error = ex.Message });
        }
    }
}
