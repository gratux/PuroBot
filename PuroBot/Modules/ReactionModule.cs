using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using ImageMagick;

namespace PuroBot.Modules
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public class ReactionModule : ModuleBase<SocketCommandContext>
	{
		private const string PlateBg = "Resources/JaySayPics/JaySayPlate_Background.png",
			PlateFingers = "Resources/JaySayPics/JaySayPlate_Fingers.png",
			WritingBg = "Resources/JaySayPics/JaySayWriting.png";

		private static readonly MagickReadSettings TextOptions = new MagickReadSettings
		{
			FontFamily = "Handgley",
			FontStyle = FontStyleType.Bold,
			TextGravity = Gravity.Center,
			BackgroundColor = MagickColors.Transparent,
			Height = 330,
			Width = 500
		};

		[Command("jaysay")]
		[Summary("pay Jay-R to write a message for you")]
		public async Task JaySayCommand([Remainder] [Summary("the message to be written")]
			string message)
		{
			try
			{
				await Context.Channel.SendFileAsync(WritingBg);
				using var bg = new MagickImage(new FileInfo(PlateBg));
				using var fingers = new MagickImage(new FileInfo(PlateFingers));
				using var text = new MagickImage($"caption:{message}", TextOptions);
				text.Rotate(-6.05d);
				bg.Composite(text, 220, 650, CompositeOperator.Over);
				bg.Composite(fingers, CompositeOperator.Over);
				await Context.Channel.SendFileAsync(new MemoryStream(bg.ToByteArray()), $"JaySays_{message}.png");
			}
			catch
			{
				await ReplyAsync("Uh Oh! Something went wrong. Please tell my owner I did a boo-boo. ;w;");
			}
		}
	}
}