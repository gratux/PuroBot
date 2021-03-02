using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PuroBot.Extensions
{
	public static class GenericExtensions
	{
		public static IEnumerable<string> GetFileTree(this DirectoryInfo d, string fileSearchPattern = "",
			bool ignoreEmptyDirs = true, bool includeExtensions = false)
		{
			var subDirs = d.GetDirectories();
			var files = d.GetFiles(fileSearchPattern);

			if (subDirs.Length == 0 && files.Length == 0 && ignoreEmptyDirs)
				yield break;

			Array.Sort(subDirs, (a, b) => Comparer.Default.Compare(a.Name, b.Name));
			Array.Sort(files, (a, b) => Comparer.Default.Compare(a.Name, b.Name));

			yield return d.Name;

			var dirLines = subDirs.Select(dir => dir.GetFileTree(fileSearchPattern, ignoreEmptyDirs, includeExtensions))
				.SelectMany(tree => tree.Select((item, index) => (index == 0 ? "├ " : "| ") + item));
			foreach (var line in dirLines) yield return line;

			var fileLines = files.Select((file, index) =>
				(index == files.Length - 1 ? "└ " : "├ ") +
				(includeExtensions ? file.Name : Path.GetFileNameWithoutExtension(file.FullName)));
			foreach (var line in fileLines) yield return line;
		}

		public static byte Scale(this byte value, double min, double max, double minScaled, double maxScaled)
		{
			var sValue = (sbyte) value;
			return (byte) (minScaled + (sValue - min) / (max - min) * (maxScaled - minScaled));
		}
	}
}