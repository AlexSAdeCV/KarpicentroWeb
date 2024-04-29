using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarpicentroWeb.Models
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Active { get; set; }
        public string NameSupplier { get; set; }
        public string NameContact { get; set; }
        public string Email { get; set; }
        public int idDirections { get; set; }

        [ForeignKey("idDirections")]
        public virtual Directions Directions { get; set; }
    }
}
