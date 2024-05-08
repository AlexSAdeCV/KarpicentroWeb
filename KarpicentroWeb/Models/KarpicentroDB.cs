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
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserDirections> UseDir { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<Materials> Materials { get; set; }
        public DbSet<ProductInter> InterProd { get; set; }
    }
}
