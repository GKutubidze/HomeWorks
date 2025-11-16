using Microsoft.EntityFrameworkCore;
using Week_19.Domain;

namespace Week_19.Data;

public class PersonContext : DbContext
{
    // Design-time constructor
    public PersonContext() { }
    
    // Runtime constructor
    public PersonContext(DbContextOptions<PersonContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Adress> Adresses { get; set; }
    
    
 
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=WEEK19_IT11;User Id=sa;Password=Amdradeongtx123;TrustServerCertificate=True;");
            // ან თუ Docker-ში გაქვთ:
            // optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PersonDb;Username=postgres;Password=yourpassword");
        }
    }
}