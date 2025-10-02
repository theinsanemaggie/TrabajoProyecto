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
        public async Task<ActionResult<IEnumerable<Dirigente>>> GetAll()
        {
            try
            {
                var dirigentes = await _db.GetAllAsync();
                if (dirigentes == null || dirigentes.Count == 0)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = "No se encontraron dirigentes." });

                return Ok(dirigentes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dirigente>> Get(int id)
        {
            try
            {
                var dirigente = await _db.GetByIdAsync(id);
                if (dirigente == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Dirigente con ID {id} no encontrado." });

                return Ok(dirigente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Add([FromBody] Dirigente d)
        {
            try
            {
                if (d == null)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "El objeto Dirigente es nulo." });

                if (d.Dni <= 0)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "El DNI debe ser válido." });

                await _db.AddAsync(d);

                return Ok(new Response { Code = "201", Message = "Dirigente creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] Dirigente d)
        {
            try
            {
                if (d == null)
                    return BadRequest(new ErrorResponse { ErrorCode = "400", Message = "Datos de dirigente inválidos." });

                var existingDirigente = await _db.GetByIdAsync(id);
                if (existingDirigente == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Dirigente con ID {id} no encontrado." });

                await _db.UpdateAsync(id, d);

                return Ok(new Response { Code = "200", Message = "Dirigente actualizado correctamente." });
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
                var existingDirigente = await _db.GetByIdAsync(id);
                if (existingDirigente == null)
                    return NotFound(new ErrorResponse { ErrorCode = "404", Message = $"Dirigente con ID {id} no encontrado." });

                await _db.DeleteAsync(id);

                return Ok(new Response { Code = "200", Message = "Dirigente eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { ErrorCode = "500", Message = $"Error interno: {ex.Message}" });
            }
        }
    }
}
