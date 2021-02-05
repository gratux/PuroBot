using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace PuroBot.Config
{
	[XmlType(nameof(GlobalConfig))]
	public struct GlobalConfig
	{
		[XmlElement(nameof(Token))] public string Token { get; set; }
	}

	[XmlType(nameof(Server))]
	public struct Server
	{
		[XmlElement(nameof(Id))] public ulong Id { get; set; }

		[XmlElement(nameof(Prefix))] public char Prefix { get; set; }

		public static bool Equals(Server lhs, Server rhs) => lhs.Id == rhs.Id && lhs.Prefix == rhs.Prefix;
	}

	[XmlInclude(typeof(Server))]
	[XmlType(nameof(ServersConfig))]
	public class ServersConfig : IList<Server>, INotifyCollectionChanged
	{
		[XmlArrayItem(typeof(Server))]
		private readonly ObservableCollection<Server> _servers = new ObservableCollection<Server>();

		public Server this[Server server] => _servers.FirstOrDefault(s => Equals(s, server));

		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add => _servers.CollectionChanged += value;
			remove => _servers.CollectionChanged -= value;
		}

		#region IList

		IEnumerator<Server> IEnumerable<Server>.GetEnumerator() => _servers.GetEnumerator();
		public IEnumerator GetEnumerator() => _servers.GetEnumerator();
		public void Add(Server item) => _servers.Add(item);
		public void Add(object item) => _servers.Add((Server) item);
		public void Clear() => _servers.Clear();
		public bool Contains(Server item) => _servers.Contains(item);
		public void CopyTo(Server[] array, int arrayIndex) => _servers.CopyTo(array, arrayIndex);
		public bool Remove(Server item) => _servers.Remove(item);
		public int Count => _servers.Count;
		public bool IsReadOnly => false;
		public int IndexOf(Server item) => _servers.IndexOf(item);
		public void Insert(int index, Server item) => _servers.Insert(index, item);
		public void RemoveAt(int index) => _servers.RemoveAt(index);

		public Server this[int index]
		{
			get => _servers[index];
			set => _servers[index] = value;
		}

		#endregion
	}
}