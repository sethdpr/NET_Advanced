using System.ComponentModel.DataAnnotations;

namespace NET_Advanced.Models
{
    public class KlantModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naam { get; set; }

        public ICollection<BestellingModel> Bestellingen { get; set; } = [];
    }
}
