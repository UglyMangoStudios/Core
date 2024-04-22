using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

	/// <summary>
	/// A class that contains all static values and references that are used everywhere.
	/// </summary>
	public static class Ref
	{
		public const char VAR_CHAR = '$';


		public static readonly IReadOnlyList<string> Random = FILL("random", "ran", "rand", "r");


		internal static IReadOnlyList<string> FILL(params string[] s) =>
			[.. s.Select(s => VAR_CHAR + s)];



		public static bool Has(this IEnumerable<string> list, string? s) =>
			s is not null && list.Any(check => check.Equals(s, StringComparison.InvariantCultureIgnoreCase));

	}
}
