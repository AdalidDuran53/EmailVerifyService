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

    public virtual DbSet<SessionLog> SessionLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

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
            entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC646784F93B");

            entity.ToTable("OperationLog");

            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");

            entity.HasOne(d => d.Session).WithMany(p => p.OperationLogs)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Operation__Respo__2B3F6F97");
        });

        modelBuilder.Entity<SessionLog>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__SessionL__C9F49270F8CBF5C6");

            entity.ToTable("SessionLog");

            entity.Property(e => e.SessionId)
                .ValueGeneratedNever()
                .HasColumnName("SessionID");
            entity.Property(e => e.EndSession).HasColumnType("datetime");
            entity.Property(e => e.InitSession).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SessionLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SessionLo__EndSe__286302EC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC0019B3D9");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456FD7CED62").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
