using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext()
    {
        //Database.EnsureDeleted(); // гарантируем, что бд удалена
        Database.EnsureCreated(); // гарантируем, что бд будет создана
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=dormitory.db");
    }
}
