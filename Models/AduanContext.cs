using Microsoft.EntityFrameworkCore;
using AduanMasyarakat.Models;

public class AduanContext : DbContext
{
    public AduanContext(DbContextOptions<AduanContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Daerah> Daerahs { get; set; }
}
