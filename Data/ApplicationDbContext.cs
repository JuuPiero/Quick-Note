using Microsoft.EntityFrameworkCore;

namespace Note.Data;

using Note.Models;

public class ApplicationDbContext : DbContext {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) {}
     public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Share> Shares { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .HasKey(m => new { m.UserId, m.GroupId });

        modelBuilder.Entity<Share>()
            .HasKey(s => new { s.NoteId, s.GroupId });

        // 🛠️ User - Note (1-to-Many)
        modelBuilder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🛠️ User - Group (1-to-Many, Owner)
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Owner)
            .WithMany(u => u.Groups)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🛠️ User - Memberships (1-to-Many)
        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🛠️ Group - Members (1-to-Many)
        modelBuilder.Entity<Member>()
            .HasOne(m => m.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(m => m.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🛠️ Note - Shares (1-to-Many)
        modelBuilder.Entity<Share>()
            .HasOne(s => s.Note)
            .WithMany(n => n.Shares)
            .HasForeignKey(s => s.NoteId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🛠️ Group - Shares (1-to-Many)
        modelBuilder.Entity<Share>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Shares)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }


}

