using System;
using back_end.Data;
using back_end.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

#nullable disable

namespace back_end.Data
{
    public partial class DataContext : DbContext,IDataContext
    {
        private readonly string _connectionString;
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DataContext(IOptions<DbConnectionInfo> dbConnectionInfo)
        {
            _connectionString = dbConnectionInfo.Value.MySqlContext;
        }
        public virtual DbSet<BotResponse> BotResponses { get; set; }
        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }
        public virtual DbSet<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
        public virtual DbSet<PlayerIngameStatus> PlayerIngameStatuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRank> UserRanks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql(_connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.13-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<BotResponse>(entity =>
            {
                entity.HasIndex(e => e.PlayerInGameStatusId, "PlayerInGameStatusID");

                entity.HasIndex(e => e.RoleId, "RoleID");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.PlayerInGameStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PlayerInGameStatusID");

                entity.Property(e => e.Response).IsRequired();

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("RoleID");

                entity.HasOne(d => d.PlayerInGameStatus)
                    .WithMany(p => p.BotResponses)
                    .HasForeignKey(d => d.PlayerInGameStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BotResponses_ibfk_2");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.BotResponses)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BotResponses_ibfk_1");
            });

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

            modelBuilder.Entity<GameSessionsUsersRole>(entity =>
            {
                entity.HasIndex(e => e.PlayerIngameStatusId, "PlayerIngameStatusID");

                entity.HasIndex(e => e.RoleId, "RoleID");

                entity.HasIndex(e => e.SessionId, "SessionID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.PlayerIngameStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PlayerIngameStatusID");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("RoleID");

                entity.Property(e => e.SessionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("SessionID");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.HasOne(d => d.PlayerIngameStatus)
                    .WithMany(p => p.GameSessionsUsersRoles)
                    .HasForeignKey(d => d.PlayerIngameStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GameSessionsUsersRoles_ibfk_4");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GameSessionsUsersRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GameSessionsUsersRoles_ibfk_3");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.GameSessionsUsersRoles)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GameSessionsUsersRoles_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameSessionsUsersRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GameSessionsUsersRoles_ibfk_1");
            });

            modelBuilder.Entity<PlayerIngameStatus>(entity =>
            {
                entity.ToTable("PlayerIngameStatus");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Role");

                entity.Property(e => e.RoleDescription).IsRequired();
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasIndex(e => e.AdminId, "AdminID");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AdminId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AdminID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.MaxNumberOfPlayers).HasColumnType("int(11)");

                entity.Property(e => e.NumberOfPlayers).HasColumnType("int(11)");

                entity.Property(e => e.RoleAutoInitialize)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.SessionName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("curdate()");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("Sessions_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UserName")
                    .IsUnique();

                entity.HasIndex(e => e.UserRankId, "UserRankID");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("curdate()");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserRankId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserRankID");

                entity.HasOne(d => d.UserRank)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRankId)
                    .HasConstraintName("Users_ibfk_1");
            });

            modelBuilder.Entity<UserRank>(entity =>
            {
                entity.HasIndex(e => e.Rank, "Rank")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Rank)
                    .IsRequired()
                    .HasColumnType("text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
