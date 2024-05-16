using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarpicentroWeb.Models
{
    public class CartBuys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int SwitchCartBuy { get; set; }
        public string Shipping { get; set; }
        public int iduserdir { get; set; }
        public int IDOrder { get; set; }
        public int idProductInter { get; set; }
        public int idUser { get; set; }

        [ForeignKey("idProductInter")]
        public virtual ProductInter ProductInter { get; set; }

        [ForeignKey("idUser")]
        public virtual User Users { get; set; }
    }
}
