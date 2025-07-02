using LMSWebAppClean.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace LMSWebAppClean.Persistence.Context
{
    public class DataDBContext : DbContext
    {
        public DataDBContext(DbContextOptions<DataDBContext> options) : base(options) {}

        public DbSet<Book> Books { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Member> Members { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
