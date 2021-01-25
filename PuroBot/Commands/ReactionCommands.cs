using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ImageMagick;

namespace PuroBot.Commands
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	internal class ReactionCommands : BaseCommandModule
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
		[Description("pay Jay-R to write a message for you")]
		public async Task JaySayCommand(CommandContext ctx, [RemainingText] [Description("the message to be written")]
			string message)
		{
			try
			{
				await ctx.RespondWithFileAsync(WritingBg);
				using var bg = new MagickImage(new FileInfo(PlateBg));
				using var fingers = new MagickImage(new FileInfo(PlateFingers));
				using var text = new MagickImage($"caption:{message}", TextOptions);
				text.Rotate(-6.05d);
				bg.Composite(text, 220, 650, CompositeOperator.Over);
				bg.Composite(fingers, CompositeOperator.Over);
				await ctx.RespondWithFileAsync($"JaySays_{message}.png", new MemoryStream(bg.ToByteArray()));
			}
			catch
			{
				await ctx.RespondAsync("Uh Oh! Something went wrong. Please tell my owner I did a boo-boo. ;w;");
			}
		}
	}
}