using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models
{
    public class Plato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public string? ImagenUrl { get; set; }

        public string? Categoria { get; set; }

        public bool Activo { get; set; } = true;
    }
}
