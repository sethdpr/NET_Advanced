using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NET_Advanced.Areas.Identity.Data;

public class NET_AdvancedUser : IdentityUser
{
    public string Voornaam { get; set; }
    public string Achternaam { get; set; }
}