using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models
{
    public class Mesa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public string Estado { get; set; } = "disponible"; // disponible, ocupada, reservada

        // Relación con reservas
        public ICollection<Reserva>? Reservas { get; set; }
    }
}
