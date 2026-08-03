using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOs.Productos;
using PedidosApi.Services.Interfaces;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController: ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll([FromQuery] bool incluirInactivos = false)
        {
            var productos = await _productoService.GetAllAsync(incluirInactivos);
            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO?>> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);

            if(producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> Create(CrearProductoDTO crearProductoDTO)
        {
            var productoCreado = await _productoService.CreateAsync(crearProductoDTO);
            return CreatedAtAction(nameof(GetById), new { id = productoCreado.Id }, productoCreado);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> Update(int id, ActualizarProductoDTO actualizarProductoDTO)
        {
            var productoActualizado = await _productoService.UpdateAsync(id, actualizarProductoDTO);

            if (productoActualizado is null)
            {
                return NotFound();
            }

            return Ok(productoActualizado);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _productoService.DeleteAsync(id);

            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
