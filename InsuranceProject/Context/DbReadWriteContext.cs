

using Microsoft.EntityFrameworkCore;

namespace InsuranceProject.Context
{
    public class DbReadWriteContext : DbContext
    {
        public DbReadWriteContext(DbContextOptions<DbReadWriteContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        
        public DbSet<ClaimAudit> ClaimAudits { get; set; }
        
    }
}