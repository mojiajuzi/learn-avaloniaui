namespace MyMoney.DatabaseService;

public class DbContextFactory
{
    public AppDbContext CreateDbContext()
    {
        return new AppDbContext();
    }
}