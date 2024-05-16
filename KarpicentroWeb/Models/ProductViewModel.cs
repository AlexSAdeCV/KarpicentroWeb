using KarpicentroWeb.Models;

namespace KarpicentroWeb.Models
{
    public class ProductViewModel
    {
        
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductInter> Inter { get; set; }
        public IEnumerable<Colors> Color { get; set; }
        public IEnumerable<Materials> Material { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public ProductInter pinter { get; set; }
    }
}
