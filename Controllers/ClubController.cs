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
        public async Task<ActionResult<IEnumerable<Club>>> GetAll()
        {
            try
            {
                var clubes = await _db.GetAllAsync();
                if (clubes == null || clubes.Count == 0)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = "No se encontraron clubes." });

                return Ok(clubes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> Get(int id)
        {
            try
            {
                var club = await _db.GetByIdAsync(id);
                if (club == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Club con ID {id} no encontrado." });

                return Ok(club);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Add([FromBody] Club c)
        {
            try
            {
                if (c == null)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "El objeto Club es nulo." });

                if (c.CantidadSocios < 0 || c.CantidadTitulos < 0)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "Cantidad de socios y títulos no puede ser negativa." });

                if (c.FechaFundacion > DateTime.Now)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "La fecha de fundación no puede ser futura." });

                await _db.AddAsync(c);

                return Ok(new Response { Code = "201", Message = "Club creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] Club c)
        {
            try
            {
                if (c == null)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "Datos de club inválidos." });

                var existingClub = await _db.GetByIdAsync(id);
                if (existingClub == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Club con ID {id} no encontrado." });

                await _db.UpdateAsync(id, c);

                return Ok(new Response { Code = "200", Message = "Club actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            try
            {
                var existingClub = await _db.GetByIdAsync(id);
                if (existingClub == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Club con ID {id} no encontrado." });

                await _db.DeleteAsync(id);

                return Ok(new Response { Code = "200", Message = "Club eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }
    }
}
