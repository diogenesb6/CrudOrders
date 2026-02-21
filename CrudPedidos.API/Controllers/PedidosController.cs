using CrudPedidos.Application.DTOs;
using CrudPedidos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPedidos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly ILogger<PedidosController> _logger;

    public PedidosController(IPedidoService pedidoService, ILogger<PedidosController> logger)
    {
        _pedidoService = pedidoService ?? throw new ArgumentNullException(nameof(pedidoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obter todos os pedidos
    /// </summary>
    /// <returns>Lista de pedidos</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> ObterTodos()
    {
        try
        {
            var pedidos = await _pedidoService.ObterTodosAsync();
            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pedidos");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "Erro ao obter pedidos", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter pedido por ID
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <returns>Pedido encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PedidoDTO>> ObterPorId(int id)
    {
        try
        {
            var pedido = await _pedidoService.ObterPorIdAsync(id);
            if (pedido == null)
                return NotFound(new { message = $"Pedido com id {id} não encontrado" });

            return Ok(pedido);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao obter pedido");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter pedido {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro ao obter pedido", error = ex.Message });
        }
    }

    /// <summary>
    /// Criar novo pedido
    /// </summary>
    /// <param name="dto">Dados do pedido a criar</param>
    /// <returns>Pedido criado</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PedidoDTO>> Criar([FromBody] CriarPedidoDTO dto)
    {
        try
        {
            var pedidoCriado = await _pedidoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = pedidoCriado.Id }, pedidoCriado);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar pedido");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pedido");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro ao criar pedido", error = ex.Message });
        }
    }

    /// <summary>
    /// Atualizar pedido existente
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <param name="dto">Dados atualizados do pedido</param>
    /// <returns>Pedido atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PedidoDTO>> Atualizar(int id, [FromBody] AtualizarPedidoDTO dto)
    {
        try
        {
            var pedidoAtualizado = await _pedidoService.AtualizarAsync(id, dto);
            return Ok(pedidoAtualizado);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar pedido");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, $"Pedido {id} não encontrado");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar pedido {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro ao atualizar pedido", error = ex.Message });
        }
    }

    /// <summary>
    /// Deletar pedido
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <returns>Confirmação de deleção</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deletar(int id)
    {
        try
        {
            var resultado = await _pedidoService.DeletarAsync(id);
            if (!resultado)
                return NotFound(new { message = $"Pedido com id {id} não encontrado" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao deletar pedido");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, $"Pedido {id} não encontrado");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao deletar pedido {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Erro ao deletar pedido", error = ex.Message });
        }
    }
}
