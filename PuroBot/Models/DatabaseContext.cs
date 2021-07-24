using Microsoft.EntityFrameworkCore;
using PuroBot.Common;
using PuroBot.Models.Domain;

namespace PuroBot.Models
{
	public class DatabaseContext : DbContext
	{
		public DbSet<ServerSettings>? Servers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
			optionsBuilder.UseNpgsql(Secrets.DbConnectionString);
	}
}