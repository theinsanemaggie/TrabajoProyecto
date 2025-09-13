using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrabajoProyecto.Data;
using TrabajoProyecto.Models;

namespace TrabajoProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClubController : ControllerBase
    {
        private readonly ClubDb _db;

        public ClubController(ClubDb db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clubes = await _db.GetAllAsync();
                return Ok(clubes);
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
                var club = await _db.GetByIdAsync(id);
                if (club == null)
                {
                    return NotFound($"Club con ID {id} no encontrado.");
                }
                return Ok(club);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Club c)
        {
            try
            {
                if (c == null)
                {
                    return BadRequest("El objeto Club es nulo.");
                }

                await _db.AddAsync(c);

                return CreatedAtAction(nameof(Get), new { id = c.ClubId }, c);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Club c)
        {
            try
            {
                // Corrected line: removed the check for c.ClubId
                if (c == null)
                {
                    return BadRequest("Datos de club inválidos.");
                }

                var existingClub = await _db.GetByIdAsync(id);
                if (existingClub == null)
                {
                    return NotFound($"Club con ID {id} no encontrado para actualizar.");
                }

                await _db.UpdateAsync(id, c);

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
                var existingClub = await _db.GetByIdAsync(id);
                if (existingClub == null)
                {
                    return NotFound($"Club con ID {id} no encontrado para eliminar.");
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