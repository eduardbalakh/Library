using Microsoft.EntityFrameworkCore;


namespace Library.Data
{
    class DictDbContext : DbContext
    {
        public DictDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; " +
                "Initial Catalog = master; " +
                "Integrated Security = True; " +
                "Connect Timeout = 30; " +
                "Encrypt = False; " +
                "TrustServerCertificate = False; " +
                "ApplicationIntent = ReadWrite; " +
                "MultiSubnetFailover = False");
        }

        public DbSet<WordEntity> Words { get; set; }
    }
}
