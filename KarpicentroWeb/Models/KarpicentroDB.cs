using Microsoft.EntityFrameworkCore;

namespace KarpicentroWeb.Models
{
    public class KarpicentroDB : DbContext
    {
        public KarpicentroDB(DbContextOptions<KarpicentroDB> options) : base(options)
        {

        }

        public DbSet<CartBuys> Cart { get; set; }
        public DbSet<Directions> Direction { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Supplier> Proveedores { get; set; }
        public DbSet<User> User { get; set; }
    }
}
