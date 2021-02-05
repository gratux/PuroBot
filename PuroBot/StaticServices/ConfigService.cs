using System.IO;
using System.Xml.Serialization;
using PuroBot.Config;

namespace PuroBot.StaticServices
{
	public static class ConfigService
	{
		private const string GlobalPath = "Config/global.xml";
		private const string ServerPath = "Config/servers.xml";

		private static GlobalConfig _global;
		private static ServersConfig _servers;
		
		public static GlobalConfig Global
		{
			get => _global;
			set
			{
				_global = value;
				Save();
			}
		}

		public static ServersConfig Servers
		{
			get => _servers;
			set
			{
				_servers = value;
				Save();
			}
		}

		static ConfigService()
		{
			Load();
			_servers.CollectionChanged += (sender, args) => Save();
		}

		private static void Save()
		{
			var globalSer = new XmlSerializer(typeof(GlobalConfig));
			using var globalWriter = new StreamWriter(GlobalPath);
			globalSer.Serialize(globalWriter, _global);

			var serversSer = new XmlSerializer(typeof(ServersConfig));
			using var serversWriter = new StreamWriter(ServerPath);
			serversSer.Serialize(serversWriter, _servers);
		}

		private static void Load()
		{
			var globalSer = new XmlSerializer(typeof(GlobalConfig));
			using var globalReader = new StreamReader(GlobalPath);
			_global = (GlobalConfig) globalSer.Deserialize(globalReader);

			var serversSer = new XmlSerializer(typeof(ServersConfig));
			using var serversReader = new StreamReader(ServerPath);
			_servers = (ServersConfig) serversSer.Deserialize(serversReader);
		}
	}
}