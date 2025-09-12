using System.ComponentModel.DataAnnotations;

namespace TrabajoProyecto.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int CantidadSocios { get; set; }

        [Range(0, int.MaxValue)]
        public int CantidadTitulos { get; set; }

        public DateTime FechaFundacion { get; set; }

        [MaxLength(150)]
        public string? UbicacionEstadio { get; set; }

        [MaxLength(120)]
        public string? NombreEstadio { get; set; }
    }
}
