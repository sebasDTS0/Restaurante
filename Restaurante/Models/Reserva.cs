using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MesaId { get; set; }

        [ForeignKey("MesaId")]
        public Mesa? Mesa { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public TimeSpan Hora { get; set; }

        public string Estado { get; set; } = "pendiente"; // pendiente, confirmada, cancelada

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
