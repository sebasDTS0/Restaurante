using Microsoft.EntityFrameworkCore;
using Restaurante.Models;

namespace Restaurante.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Plato> Plato { get; set; }
    }
}
