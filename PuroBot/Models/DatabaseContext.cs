using Discord;
using Microsoft.EntityFrameworkCore;
using PuroBot.Common;
using PuroBot.Models.Domain;

namespace PuroBot.Models
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<ServerSettings>? Servers { get; set; }
        public DbSet<BotActivity>? Activities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Secrets.DbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerSettings>(entity =>
            {
                entity.ToTable("Server");
                entity.HasKey(serverSettings => serverSettings.Id);

                entity.Property(serverSettings => serverSettings.Id)
                    .HasColumnName("ServerId")
                    .HasColumnType("bigserial")
                    .IsRequired();

                entity.Property(serverSettings => serverSettings.Prefix)
                    .HasColumnName("CommandPrefix")
                    .HasColumnType("character varying(10)")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<BotActivity>(entity =>
            {
                entity.ToTable("Activity");
                entity.HasKey(activity => activity.Id);

                entity.Property(botActivity => botActivity.Id)
                    .HasColumnName("Id")
                    .HasColumnType("serial")
                    .IsRequired();

                entity.Property(botActivity => botActivity.Name)
                    .HasColumnName("Name")
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(botActivity => botActivity.Type)
                    .HasColumnName("Type")
                    .HasColumnType("activitytype")
                    .IsRequired()
                    .HasDefaultValue(ActivityType.Playing)
                    .HasConversion<string>();
            });
        }
    }
}
