using Microsoft.EntityFrameworkCore;
using PuroBot.Common;
using PuroBot.Models.Domain;

namespace PuroBot.Models
{
	public class DatabaseContext : DbContext
	{
		public DbSet<ServerSettings>? Servers { get; set; }
		public DbSet<BotActivity>? Activities { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
			optionsBuilder.UseNpgsql(Secrets.DbConnectionString);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<BotActivity>()
				.Property(botActivity => botActivity.Type)
				.HasConversion<string>();
		}
	}
}