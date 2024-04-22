using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
	public static class FileUtility
	{

		public static string[] ReadAllLines(string path)
		{
			if (!File.Exists(path)) return Array.Empty<string>();
			return File.ReadAllLines(path);
		}


	}
}
