using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SimpleProject.Domain.Enums;
using SimpleProject.Domain;

namespace SimpleProject.Data;
public class AppDbContext : DbContext
{
    public DbSet<AdminRole>? AdminRole { get; set; }
    public DbSet<AdminUser>? AdminUser { get; set; }
    public DbSet<Brand>? Brand { get; set; }
    public DbSet<CustomField>? CustomField { get; set; }
    public DbSet<CustomFieldOption>? CustomFieldOption { get; set; }
    public DbSet<CustomFieldValue>? CustomFieldValue { get; set; }
    public DbSet<ExcelUpload>? ExcelUpload { get; set; }
    public DbSet<Animal> Animal { get; set; } = null!;
    public DbSet<Collar> Collars { get; set; } = null!;
    public DbSet<ScanEvent> ScanEvents { get; set; } = null!;
    public DbSet<FoundReport> FoundReports { get; set; } = null!;
    public DbSet<PetImage> PetImages { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<Dealer> Dealers { get; set; } = null!;
    public DbSet<CodeBatch> CodeBatches { get; set; } = null!;
    public DbSet<CodeAssigment> CodeAssigments { get; set; } = null!;
    public DbSet<QrOwnership> QrOwnerships { get; set; } = null!;
    public DbSet<QrTransferTicket> QrTransferTickets { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminUser>().ToTable("AdminUser","dbo");
        modelBuilder.Entity<AdminRole>().ToTable("AdminRole","dbo");

        modelBuilder.Entity<AdminUser>(e =>
        {
            e.Property(x => x.UserName).HasMaxLength(100);
            e.Property(x => x.Password).HasMaxLength(256);
            e.Property(x => x.Name).HasMaxLength(150);
            e.Property(x => x.Surname).HasMaxLength(150);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.Status).HasConversion<int>();
            e.HasOne(x => x.AdminRole).WithMany(r => r.AdminUsers).HasForeignKey(x => x.AdminRoleId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AdminRole>(e =>
        {
            e.Property(x => x.Code).HasMaxLength(100);
            e.Property(x => x.Name).HasMaxLength(150);
            e.Property(x => x.Settings);
            e.Property(x => x.Status).HasConversion<int>();
        });

        modelBuilder.Entity<AdminUser>().Ignore(a => a.EntityLogs);
        modelBuilder.Entity<AdminUser>().Ignore(a => a.ErrorLogs);

        modelBuilder.Entity<Animal>().ToTable("Hayvan","dbo");

        modelBuilder.Entity<Collar>(e =>
        {
            e.ToTable("Collar","dbo");
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Code).HasMaxLength(64).IsRequired();
            e.Property(x => x.AssetType).HasMaxLength(20).IsRequired();
            e.Property(x => x.FriendlyName).HasMaxLength(120);
            e.HasOne(x => x.Animal).WithMany().HasForeignKey(x => x.AnimalId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.OwnerUser).WithMany().HasForeignKey(x => x.OwnerUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ScanEvent>(e =>
        {
            e.ToTable("ScanEvent","dbo");
            e.HasOne(x => x.Collar).WithMany().HasForeignKey(x => x.CollarId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Animal).WithMany().HasForeignKey(x => x.AnimalId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FoundReport>(e =>
        {
            e.ToTable("FoundReport","dbo");
            e.HasOne(x => x.Collar).WithMany().HasForeignKey(x => x.CollarId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Animal).WithMany().HasForeignKey(x => x.AnimalId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PetImage>(e =>
        {
            e.ToTable("PetImage","dbo");
            e.HasOne(x => x.Animal).WithMany().HasForeignKey(x => x.AnimalId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notification","dbo");
        });

        modelBuilder.Entity<Subject>(e =>
        {
            e.ToTable("Subject","dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Type).HasMaxLength(20).IsRequired();
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.Notes).HasMaxLength(1000);
            e.Property(x => x.FotoUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<Dealer>(e =>
        {
            e.ToTable("Dealer","dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Code).HasMaxLength(50).IsRequired();
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Contact).HasMaxLength(150);
        });

        modelBuilder.Entity<CodeBatch>(e =>
        {
            e.ToTable("CodeBatch","dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.BatchCode).HasMaxLength(50).IsRequired();
            e.HasOne(x => x.Dealer).WithMany().HasForeignKey(x => x.DealerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CodeAssigment>(e =>
        {
            e.ToTable("CodeAssignment","dbo");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.CodeBatch).WithMany().HasForeignKey(x => x.BatchId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Collar).WithMany().HasForeignKey(x => x.CollarId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QrOwnership>(e =>
        {
            e.ToTable("QrOwnership","dbo");
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Collar).WithMany().HasForeignKey(x => x.CollarId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.OwnerUser).WithMany().HasForeignKey(x => x.OwnerUserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QrTransferTicket>(e =>
        {
            e.ToTable("QrTransferTicket","dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).HasMaxLength(20).IsRequired();
            e.Property(x => x.Token).HasMaxLength(64).IsRequired();
            e.HasOne(x => x.Collar).WithMany().HasForeignKey(x => x.CollarId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.FromOwnerUser).WithMany().HasForeignKey(x => x.FromOwnerUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ToOwnerUser).WithMany().HasForeignKey(x => x.ToOwnerUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.FromDealer).WithMany().HasForeignKey(x => x.FromDealerId).OnDelete(DeleteBehavior.Restrict);
        });

        BaseEntityDefaults(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Conventions.Remove<SqlServerOnDeleteConvention>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeAsUtcValueConverter>();
        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeAsUtcValueConverter>();
    }

    private class DateTimeAsUtcValueConverter() : ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    private class NullableDateTimeAsUtcValueConverter() : ValueConverter<DateTime?, DateTime?>(v => v.HasValue ? v.Value.ToUniversalTime() : null, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);

    private static void BaseEntityDefaults(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(e => typeof(Entity).IsAssignableFrom(e.ClrType)))
        {
            if (modelBuilder.Entity(entity.Name).Metadata.FindProperty(Consts.Status) != null)
            {
                modelBuilder.Entity(entity.Name).Property(Consts.Status).HasDefaultValueSql(((int)Status.ACTIVE).ToString()).HasSentinel(Status.ACTIVE);
            }
            modelBuilder.Entity(entity.Name).Property("Deleted").HasDefaultValue(false);
            modelBuilder.Entity(entity.Name).Property("CreateDate").HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity(entity.Name).Property("UpdateDate").HasDefaultValueSql("GETUTCDATE()");
        }
    }
}