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
			Array.Sort(subDirs, (a, b) => Comparer.Default.Compare(a.Name, b.Name));
			var files = d.GetFiles(fileSearchPattern);
			Array.Sort(files, (a, b) => Comparer.Default.Compare(a.Name, b.Name));

			if (subDirs.Length == 0 && files.Length == 0 && ignoreEmptyDirs) yield return "";

			yield return d.Name;

			foreach (var subDir in subDirs)
			{
				// ReSharper disable PossibleMultipleEnumeration
				var subTree = subDir.GetFileTree(fileSearchPattern, ignoreEmptyDirs);
				for (var i = 0; i < subTree.Count(); i++)
					if (i == 0)
						yield return "├ " + subTree.ElementAt(i);
					else
						yield return "| " + subTree.ElementAt(i);
				// ReSharper restore PossibleMultipleEnumeration
			}

			for (var i = 0; i < files.Length; i++)
			{
				var filename = includeExtensions ? files[i].Name : Path.GetFileNameWithoutExtension(files[i].FullName);
				var symbol = i == files.Length - 1 ? "└ " : "├ ";
				yield return symbol + filename;
			}
		}
	}
}