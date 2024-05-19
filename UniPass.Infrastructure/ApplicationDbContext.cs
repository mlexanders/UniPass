using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure.Base;
using UniPass.Infrastructure.Models;

namespace UniPass.Infrastructure;

public class ApplicationDbContext : DbContextBase
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUserProfile> Profiles { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Team>()
            .HasMany<ApplicationUser>(u => u.Workers)
            .WithMany(t => t.Teams);

        builder.Entity<Team>()
            .HasOne<ApplicationUser>(u => u.Organizer)
            .WithMany(u => u.CreatedTeams)
            .HasForeignKey(u => u.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(builder);
    }
}