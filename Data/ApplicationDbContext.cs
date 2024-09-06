using Microsoft.EntityFrameworkCore;
using MyApp.Entity;

namespace MyApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<users> users { get; set; }
        public DbSet<categories> categories { get; set; }
        public DbSet<products> products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<orderDetail> orderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<users>(entity =>
            {
                entity.HasKey(e => e.userId);
            });
        }
    }
}
