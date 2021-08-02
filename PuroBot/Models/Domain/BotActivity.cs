using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;

namespace PuroBot.Models.Domain
{
	[Table("Activity")]
	public class BotActivity
	{
		[Key]
		[Column("Id", TypeName = "serial")]
		public int Id { get; set; }

		[Column("Name", TypeName = "text")] public string? Name { get; set; }

		[Column("Type", TypeName = "activitytype")]
		public ActivityType Type { get; set; }
	}
}