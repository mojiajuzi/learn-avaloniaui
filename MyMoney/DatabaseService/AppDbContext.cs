using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMoney.Models;

namespace MyMoney.DatabaseService;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public DbSet<Tag> Tags { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Contact> Contacts { get; set; } = null!;

    public DbSet<ContactTag> ContactTags { get; set; } = null!;

    public DbSet<Work> Works { get; set; } = null!;

    public DbSet<Expense> Expenses { get; set; } = null!;

    public DbSet<WorkContact> WorkContacts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MyMoney",
                "mymoney.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath};Foreign Keys=False");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ContactTag>(entity =>
        {
            entity.ToTable("contact_tags");
            entity.HasKey(ct => new { ct.ContactId, ct.TagId });

            entity.HasOne(ct => ct.Contact)
                .WithMany(c => c.ContactTags)
                .HasForeignKey(ct => ct.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ct => ct.Tag)
                .WithMany(t => t.ContactTags)
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.StartAt).IsRequired();
            entity.Property(e => e.EndAt);
            entity.Property(e => e.ExceptionAt);
            entity.Property(e => e.TotalMoney)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.ReceivingPayment)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.CostMoney)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue(WorkStatus.PreStart);
        });

        modelBuilder.Entity<WorkContact>(entity =>
        {
            entity.HasKey(e => new { e.WorkId, e.ContactId });

            entity.HasOne(wc => wc.Work)
                .WithMany(w => w.WorkContacts)
                .HasForeignKey(wc => wc.WorkId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(wc => wc.Contact)
                .WithMany()
                .HasForeignKey(wc => wc.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.Remark)
                .HasMaxLength(200);

            entity.Property(e => e.Role)
                .IsRequired()
                .HasDefaultValue(WorkContactRole.Other)
                .HasConversion<string>();
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            entity.Property(e => e.Date)
                .IsRequired();
            entity.Property(e => e.InCome)
                .IsRequired();

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Work)
                .WithMany(w => w.Expenses)
                .HasForeignKey(e => e.WorkId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseModel &&
                        (e.State == EntityState.Modified || e.State == EntityState.Added));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseModel)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.Now;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseModel &&
                        (e.State == EntityState.Modified || e.State == EntityState.Added));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseModel)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.Now;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}