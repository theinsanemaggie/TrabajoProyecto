using System.ComponentModel.DataAnnotations;

namespace TrabajoProyecto.Models
{
    public class Dirigente
    {
        public int DirigenteId { get; set; }
        public int ClubId { get; set; }

        [Required, MaxLength(80)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string Apellido { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        [Required, MaxLength(80)]
        public string Rol { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Dni { get; set; }
    }
}
