using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarpicentroWeb.Models
{
    public class Directions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? NameUser { get; set; }
        public string? Street { get; set; }
        public string? Neighborhood { get; set; }
        public string? ZipCode { get; set; }
        public string? Delegations { get; set; }
        public string State { get; set; }
        public int Active { get; set; }
        public int idUser { get; set; }

        [ForeignKey("idUser")]
        public virtual User User { get; set; }
    }
}
