using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace NET_Advanced.Models
{
    public class BestellingModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Naam { get; set; }
        [Required]
        public int KlantId { get; set; }
        public KlantModel? Klant { get; set; }
        public List<BestellingProductModel> BestellingProducten { get; set; } = new();
        public string GemaaktDoor { get; set; }
        public DateTime AangemaaktOp { get; set; } = DateTime.Now;
    }
}