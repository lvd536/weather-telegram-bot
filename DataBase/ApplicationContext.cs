namespace TgBotPractice.DataBase;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
 
public class Human
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public string City { get; set; } = null!;
    public bool Autosend { get; set; }
    public bool IsAdmin { get; set; }
    
    public DateTime AutoWeather = new DateTime();
}

public class ApplicationContext : DbContext
{
    public DbSet<Human> Users => Set<Human>();
    public ApplicationContext()
    {
        Database.Migrate();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=database.db");
    }
}

public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlite("Data Source=database.db");
        
        return new ApplicationContext();
    }
}