using Microsoft.EntityFrameworkCore;

using VLimat.Eduz.Domain.Features.Masters;

namespace VLimat.Eduz.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MasterConfig> MasterConfigs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // add entity configuration here if needed
        }
    }
}