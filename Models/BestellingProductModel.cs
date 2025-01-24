using System.ComponentModel.DataAnnotations;

namespace NET_Advanced.Models
{
    public class BestellingProductModel
    {
        public int BestellingId { get; set; }
        public BestellingModel Bestelling { get; set; }

        public int ProductId { get; set; }
        public ProductModel Product { get; set; }

        [Range(1, 1000)]
        public int Aantal { get; set; }
    }
}
