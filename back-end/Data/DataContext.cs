using System;
using System.Collections.Generic;
using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace back_end.Data
{
    public partial class DataContext : DbContext,IDataContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }
        public virtual DbSet<GameResult> GameResults { get; set; }
        public virtual DbSet<GameSessionUser> GameSessionUsers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=mysql-kanan.alwaysdata.net;user=kanan;password=kanan200212345;database=kanan_mafiadb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.13-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<EfmigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__EFMigrationsHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<GameResult>(entity =>
            {
                entity.HasIndex(e => e.GameSessionUsersId, "GameSessionUsersId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.GameSessionUsersId).HasColumnType("int(11)");

                entity.Property(e => e.IsWinner).HasColumnType("bit(1)");

                entity.HasOne(d => d.GameSessionUsers)
                    .WithMany(p => p.GameResults)
                    .HasForeignKey(d => d.GameSessionUsersId)
                    .HasConstraintName("GameResults_ibfk_1");
            });

            modelBuilder.Entity<GameSessionUser>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "RoleId");

                entity.HasIndex(e => e.SessionId, "SessionId");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.SessionId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GameSessionUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("GameSessionUsers_ibfk_2");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.GameSessionUsers)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("GameSessionUsers_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameSessionUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("GameSessionUsers_ibfk_3");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Rolename).IsRequired();
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.SessionDescription).IsRequired();

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
