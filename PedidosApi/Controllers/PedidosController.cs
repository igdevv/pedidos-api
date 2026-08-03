using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOs.Pedidos;
using PedidosApi.Services.Interfaces;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController: ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;            
        }

        [HttpGet]

        public async Task<ActionResult<PedidoDTO>> GetAll()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoDTO>> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);

            if (pedido is null)
            {
                return NotFound(new
                {
                    mensaje = $"No existe un pedido con el ID {id}."
                });
            }

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> Create(
            CrearPedidoDTO crearPedidoDTO)
        {
            try
            {
                var pedidoCreado = await _pedidoService
                    .CreateAsync(crearPedidoDTO);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = pedidoCreado.Id },
                    pedidoCreado);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new
                {
                    mensaje = exception.Message
                });
            }
        }
    }
}

