using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using PuroBot.Config;

namespace PuroBot.StaticServices
{
	public static class ConfigService
	{
		private const string GlobalPath = "Config/global.xml";
		private const string ServerPath = "Config/servers.xml";

		private static ObservableCollection<Server> _servers;

		public static GlobalConfig Global { get; private set; }

		public static ObservableCollection<Server> Servers
		{
			get => _servers;
			private set
			{
				_servers = value;
				_servers.CollectionChanged += (sender, args) => Save();
			}
		}

		static ConfigService()
		{
			Load();
		}

		private static void Save()
		{
			var globalSer = new XmlSerializer(typeof(GlobalConfig));
			using var globalWriter = new StreamWriter(GlobalPath);
			globalSer.Serialize(globalWriter, Global);

			var serversSer = new XmlSerializer(typeof(ObservableCollection<Server>));
			using var serversWriter = new StreamWriter(ServerPath);
			serversSer.Serialize(serversWriter, _servers);
		}

		private static void Load()
		{
			var globalSer = new XmlSerializer(typeof(GlobalConfig));
			using var globalReader = new StreamReader(GlobalPath);
			Global = (GlobalConfig) globalSer.Deserialize(globalReader);
			
			var serversSer = new XmlSerializer(typeof(ObservableCollection<Server>));
			using var serversReader = new StreamReader(ServerPath);
			Servers = (ObservableCollection<Server>) serversSer.Deserialize(serversReader);
		}
	}
}