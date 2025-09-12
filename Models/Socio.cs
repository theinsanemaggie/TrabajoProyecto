using System.ComponentModel.DataAnnotations;

namespace TrabajoProyecto.Models
{
    public class Socio
    {
        public int SocioId { get; set; }
        public int ClubId { get; set; }

        [Required, MaxLength(80)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string Apellido { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAsociado { get; set; }

        [Range(1, int.MaxValue)]
        public int Dni { get; set; }

        [Range(0, int.MaxValue)]
        public int CantidadAsistencias { get; set; }
    }
}
