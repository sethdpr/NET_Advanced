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
        private decimal _prijs;
        public decimal Prijs
        {
            get
            {
                return _prijs;
            }
            set
            {
                _prijs = value / 100;
            }
        }
    }
}