using System.Data.Entity;

namespace Example.Ef
{
    public class Context : DbContext
    {
        public Context(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IDbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().ToTable("Test")
                .Property(test => test.Col1).HasColumnName("col1");
        }
    }
}