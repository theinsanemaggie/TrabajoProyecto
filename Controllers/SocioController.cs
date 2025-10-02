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
        public async Task<ActionResult<IEnumerable<Socio>>> GetAll()
        {
            try
            {
                var socios = await _db.GetAllAsync();
                if (socios == null || socios.Count == 0)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = "No se encontraron socios." });

                return Ok(socios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Socio>> Get(int id)
        {
            try
            {
                var socio = await _db.GetByIdAsync(id);
                if (socio == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Socio con ID {id} no encontrado." });

                return Ok(socio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Add([FromBody] Socio s)
        {
            try
            {
                if (s == null)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "El objeto Socio es nulo." });

                if (s.FechaAsociado < s.FechaNacimiento)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "La fecha de asociación no puede ser anterior al nacimiento." });

                if (s.CantidadAsistencias < 0)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "Cantidad de asistencias no puede ser negativa." });

                await _db.AddAsync(s);

                return Ok(new Response { Code = "201", Message = "Socio creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] Socio s)
        {
            try
            {
                if (s == null)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "Datos de socio inválidos." });

                var existingSocio = await _db.GetByIdAsync(id);
                if (existingSocio == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Socio con ID {id} no encontrado." });

                await _db.UpdateAsync(id, s);

                return Ok(new Response { Code = "200", Message = "Socio actualizado correctamente." });
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
                var existingSocio = await _db.GetByIdAsync(id);
                if (existingSocio == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Socio con ID {id} no encontrado." });

                await _db.DeleteAsync(id);

                return Ok(new Response { Code = "200", Message = "Socio eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }
    }
}
