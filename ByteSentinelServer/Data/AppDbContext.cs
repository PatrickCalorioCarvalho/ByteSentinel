using ByteSentinelServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ByteSentinelServer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Agent> Agents => Set<Agent>();
        public DbSet<SystemMetric> SystemMetrics => Set<SystemMetric>();
        public DbSet<DiskMetric> DiskMetrics => Set<DiskMetric>();
        public DbSet<ContainerMetric> ContainerMetrics => Set<ContainerMetric>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemMetric>()
                .HasMany(x => x.Disks)
                .WithOne()
                .HasForeignKey(d => d.SystemMetricId);
        }
    }
}
