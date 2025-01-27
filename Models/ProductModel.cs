using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NET_Advanced.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naam { get; set; }
        public decimal Prijs { get; set; }
    }
}