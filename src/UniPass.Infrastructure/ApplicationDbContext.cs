using Microsoft.EntityFrameworkCore;
using UniPass.Domain;
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
    public DbSet<Folder> Folders { get; set; } = null!;
    public DbSet<Key> Keys { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Team>()
            .HasMany<ApplicationUser>(u => u.Workers)
            .WithMany(t => t.Teams);

        builder.Entity<Team>()
            .HasOne<ApplicationUser>(u => u.Organizer)
            .WithMany(u => u.CreatedTeams)
            .HasForeignKey(u => u.OrganizerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Folder>()
            .HasOne<ApplicationUser>()
            .WithMany(u => u.Folders)
            .HasForeignKey(f => f.OwnerId);

        builder.Entity<Team>()
            .HasMany<Folder>(t => t.Folders)
            .WithOne()
            .HasForeignKey(f => f.TeamId);
        
        base.OnModelCreating(builder);
    }
}