using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmailVerifyService.Models;

public partial class EmailVerifyServiceDbContext : DbContext
{
    public EmailVerifyServiceDbContext()
    {
    }

    public EmailVerifyServiceDbContext(DbContextOptions<EmailVerifyServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OperationLog> OperationLogs { get; set; }

    public virtual DbSet<StatusCode> StatusCodes { get; set; }

    public virtual DbSet<VerifyCode> VerifyCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC64B675C3A4");

            entity.ToTable("OperationLog");

            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<StatusCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StatusCo__3214EC27F11C66BB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusDescription).HasMaxLength(100);
        });

        modelBuilder.Entity<VerifyCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VerifyCo__3214EC27EB0703C6");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.VerifyStatus).HasDefaultValue(1);

            entity.HasOne(d => d.VerifyStatusNavigation).WithMany(p => p.VerifyCodes)
                .HasForeignKey(d => d.VerifyStatus)
                .HasConstraintName("FK__VerifyCod__Verif__29572725");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
