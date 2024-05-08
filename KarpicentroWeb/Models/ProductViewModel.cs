using KarpicentroWeb.Models;

namespace KarpicentroWeb.Models
{
    public class ProductViewModel
    {
        
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductInter> Inter { get; set; }
        public IEnumerable<Colors> Colors { get; set; }
        public IEnumerable<Materials> Materials { get; set; }
    }
}
