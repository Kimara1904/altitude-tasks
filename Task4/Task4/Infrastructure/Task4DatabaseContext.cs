using Microsoft.EntityFrameworkCore;
using Task4.Models;

namespace Task4.Infrastructure
{
    public class Task4DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public Task4DatabaseContext(DbContextOptions<Task4DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Task4DatabaseContext).Assembly);
        }
    }
}
