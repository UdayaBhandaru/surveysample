using Identity.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Service.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private IConfiguration configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(string.IsNullOrEmpty(this.configuration.GetValue<string>("DatabaseSchema")) ? "dbo" : this.configuration.GetValue<string>("DatabaseSchema"));
            base.OnModelCreating(builder);
            builder.Entity<UserProfile>().ToTable("UserProfile");
            builder.Entity<ApplicationUser>().ToTable("IDENTITYUSER").Property(p => p.ConcurrencyStamp).HasMaxLength(450);
            builder.Entity<IdentityUserClaim<string>>().ToTable("IDENTITYUSERCLAIM");
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
