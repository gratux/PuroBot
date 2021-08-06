using PuroBot.Common;

namespace PuroBot.Models.Domain
{
    public class ServerSettings
    {
        public ServerSettings(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; }
        public string Prefix { get; set; } = Settings.DefaultPrefix;
    }
}
