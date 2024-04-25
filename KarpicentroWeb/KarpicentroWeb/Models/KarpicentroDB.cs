using Microsoft.EntityFrameworkCore;

namespace KarpicentroWeb.Models
{
    public class KarpicentroDB : DbContext
    {
        public KarpicentroDB(DbContextOptions<KarpicentroDB> options) : base(options)
        {

        }

        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Venta> Venta { get; set; }
    }
}
