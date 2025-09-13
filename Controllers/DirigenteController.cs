using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrabajoProyecto.Data;
using TrabajoProyecto.Models;

namespace TrabajoProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DirigenteController : ControllerBase
    {
        private readonly DirigenteDb _db;

        public DirigenteController(DirigenteDb db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dirigentes = await _db.GetAllAsync();
                return Ok(dirigentes);
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
                var dirigente = await _db.GetByIdAsync(id);
                if (dirigente == null)
                {
                    return NotFound($"Dirigente con ID {id} no encontrado.");
                }
                return Ok(dirigente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Dirigente d)
        {
            try
            {
                if (d == null)
                {
                    return BadRequest("El objeto Dirigente es nulo.");
                }

                await _db.AddAsync(d);

                return CreatedAtAction(nameof(Get), new { id = d.DirigenteId }, d);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Dirigente d)
        {
            try
            {
                // Corrected code: Remove the check for id != d.DirigenteId
                if (d == null)
                {
                    return BadRequest("Datos de dirigente inválidos.");
                }

                var existingDirigente = await _db.GetByIdAsync(id);
                if (existingDirigente == null)
                {
                    return NotFound($"Dirigente con ID {id} no encontrado para actualizar.");
                }

                await _db.UpdateAsync(id, d);

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
                var existingDirigente = await _db.GetByIdAsync(id);
                if (existingDirigente == null)
                {
                    return NotFound($"Dirigente con ID {id} no encontrado para eliminar.");
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