using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOs.Pedidos;
using PedidosApi.Services.Interfaces;
using System.Security.Claims;

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
        [Authorize]
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

        [HttpGet("mi-identidad")]
        public IActionResult MiIdentidad()
        {
            var usuarioIdTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);
       
            if(!int.TryParse(usuarioIdTexto, out int usuarioId))
            {
                return Unauthorized();
            }

            var nombre = User.FindFirstValue(ClaimTypes.Name);
            var correo = User.FindFirstValue(ClaimTypes.Email);
            var rol = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                usuarioId,
                nombre,
                correo,
                rol
            });

        }

      
    }
}

