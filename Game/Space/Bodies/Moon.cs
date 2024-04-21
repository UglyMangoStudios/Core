using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Game.Space.Bodies
{
	/// <summary>
	/// Simple data representation for a moon.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	[Table("moon")]
	public class Moon : BaseBody
    {

		/// <summary>
		/// Constructor for EF Core. Not safe to use.
		/// </summary>
		protected Moon() { }


		public Moon(double radius, string? id = null, string? name = null, string? alias = null, string? description = null) :
            base(radius, id, name ?? "Unnamed Moon", alias, description)
        {

		}


	}
}
