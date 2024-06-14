using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarpicentroWeb.Models
{
    public class ProductInter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int idProducts { get; set; }
        public int idColors { get; set; }
        public int idMaterials { get; set; }
        public int Stock { get; set; }
        public decimal SalePrice { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("idProducts")]
        public virtual Product Product { get; set; }

        [ForeignKey("idColors")]
        public virtual Colors Colors { get; set; }

        [ForeignKey("idMaterials")]
        public virtual Materials Materials { get; set; }
    }
}
