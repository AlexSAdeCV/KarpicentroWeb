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
        public int idProduct { get; set; }

        [ForeignKey("idProduct")]
        public virtual Product Products { get; set; }
    }
}
