using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarpicentroWeb.Models
{
    public class UserDirections
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int idUser { get; set; }
        public int idDirections { get; set; }

        [ForeignKey("idUser")]
        public virtual User User { get; set; }

        [ForeignKey("idDirections")]
        public virtual Directions Directions { get; set; }
    }
}
