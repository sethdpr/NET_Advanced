using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NET_Advanced.Areas.Identity.Data;
using NET_Advanced.Models;

namespace NET_Advanced.Data;

public class IdentityContext : IdentityDbContext<NET_AdvancedUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    public DbSet<KlantModel> Klanten { get; set; }
    public DbSet<ProductModel> Producten { get; set; }
    public DbSet<BestellingModel> Bestellingen { get; set; }
    public DbSet<API> API { get; set; }
}
