namespace NET_Advanced.Models
{
   public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public IList<string> Roles { get; set; }
    }
}
