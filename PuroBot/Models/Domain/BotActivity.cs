using Discord;

namespace PuroBot.Models.Domain
{
    public class BotActivity
    {
        public BotActivity(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public string? Name { get; set; }
        public ActivityType Type { get; set; }
    }
}
