using Microsoft.EntityFrameworkCore;

namespace Example.Ef
{
    public class Context : DbContext
    {
        private readonly string connectionString;

        public Context(DbContextOptions options)
            : base(options)
        {
        }

        public Context(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().ToTable("Test")
                .Property(test => test.Col1).HasColumnName("col1");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrWhiteSpace(this.connectionString) == false)
            {
                optionsBuilder.UseSqlServer(
                    this.connectionString,
                    builder => builder.UseRowNumberForPaging());
            }
        }
    }
}