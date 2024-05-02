using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarpicentroWeb.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Active { get; set; }
        public string Name { get; set; }
        public string ProductFeatures { get; set; }
        public string Description { get; set; }
        public decimal PurchaseProduct { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public string Categories { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public int idSupplier { get; set; }

        [ForeignKey("idSupplier")]
        public virtual Supplier Suppliers { get; set; }
    }
}
