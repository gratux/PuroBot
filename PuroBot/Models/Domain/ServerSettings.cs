using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PuroBot.Common;

namespace PuroBot.Models.Domain
{
	[Table("Server")]
	public class ServerSettings
	{
		public ServerSettings(ulong id) => Id = id;

		[Key]
		[Column("ServerId", TypeName = "bigserial")]
		public ulong Id { get; set; }

		[MaxLength(10)]
		[Column("CommandPrefix", TypeName = "character varying(10)")]
		public string Prefix { get; set; } = Settings.DefaultPrefix;
	}
}