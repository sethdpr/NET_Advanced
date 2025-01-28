using NET_Advanced.Models;

namespace NET_Advanced.ViewModels
{
    // This class is used to display a list of orders for a specific customer
    public class BestellingIndexViewModel
    {
        public int KlantId { get; set; }
        public string KlantNaam { get; set; }
        public IEnumerable<BestellingModel> Bestellingen { get; set; }
    }
}
