using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrabajoProyecto.Data;
using TrabajoProyecto.Models;

namespace TrabajoProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SocioController : ControllerBase
    {
        private readonly SocioDb _db;

        public SocioController(SocioDb db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var socios = await _db.GetAllAsync();
                return Ok(socios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var socio = await _db.GetByIdAsync(id);
                if (socio == null)
                {
                    return NotFound($"Socio con ID {id} no encontrado.");
                }
                return Ok(socio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Socio s)
        {
            try
            {
                if (s == null)
                {
                    return BadRequest("El objeto Socio es nulo.");
                }

                await _db.AddAsync(s);

                return CreatedAtAction(nameof(Get), new { id = s.SocioId }, s);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Socio s)
        {
            try
            {
                // The fix is here. You only need to check if the body is null.
                if (s == null)
                {
                    return BadRequest("Datos de socio inválidos.");
                }

                var existingSocio = await _db.GetByIdAsync(id);
                if (existingSocio == null)
                {
                    return NotFound($"Socio con ID {id} no encontrado para actualizar.");
                }

                await _db.UpdateAsync(id, s);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingSocio = await _db.GetByIdAsync(id);
                if (existingSocio == null)
                {
                    return NotFound($"Socio con ID {id} no encontrado para eliminar.");
                }

                await _db.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}