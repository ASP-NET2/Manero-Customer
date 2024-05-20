using Manero_Customer.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Manero_Customer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<AccountUserEntity> AccountUser { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AccountUserEntity>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<AccountUserEntity>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
