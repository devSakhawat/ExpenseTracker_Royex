using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Context;

public class ExpenseTrackerContext : DbContext
{
  public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) : base(options) { }

  public DbSet<Category> Categories { get; set; }
  public DbSet<Expense> Expenses { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<RefreshToken> RefreshTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Category
    modelBuilder.Entity<Category>(entity =>
    {
      entity.HasKey(e => e.CategoryId);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
      entity.Property(e => e.Description).HasMaxLength(500);
      entity.HasIndex(e => e.Name).IsUnique();
    });

    // Expense
    modelBuilder.Entity<Expense>(entity =>
    {
      entity.HasKey(e => e.ExpenseId);
      entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
      entity.Property(e => e.Description).HasMaxLength(1000);
      entity.HasOne(e => e.Category)
        .WithMany(c => c.Expenses)
        .HasForeignKey(e => e.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);
      entity.HasIndex(e => e.ExpenseDate);
      entity.HasIndex(e => e.CategoryId);
    });

    // User
    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.UserId);
      entity.Property(e => e.LoginId).IsRequired().HasMaxLength(50);
      entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
      entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
      entity.Property(e => e.Email).HasMaxLength(200);
      entity.HasIndex(e => e.LoginId).IsUnique();
    });

    // RefreshToken
    modelBuilder.Entity<RefreshToken>(entity =>
    {
      entity.HasKey(e => e.RefreshTokenId);
      entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
      entity.Property(e => e.CreatedByIp).HasMaxLength(50);
      entity.Property(e => e.ReplacedByToken).HasMaxLength(500);
      entity.HasOne(e => e.User)
        .WithMany(u => u.RefreshTokens)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Cascade);
      entity.HasIndex(e => e.Token);
      entity.Ignore(e => e.IsActive);
    });
  }
}
