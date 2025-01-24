using NET_Advanced.Models;

namespace NET_Advanced.ViewModels
{
    public class BestellingIndexViewModel
    {
        public int KlantId { get; set; }
        public string KlantNaam { get; set; }
        public IEnumerable<BestellingModel> Bestellingen { get; set; }
    }
}
