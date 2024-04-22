using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Space.Bodies
{

	public enum StarType
	{
		MainSequence, Blackhole, Pulsar
	}


	/// <summary>
	/// A representation of a star object
	/// </summary>
	
	[JsonObject(MemberSerialization.OptIn)]
	[Table("star")]
	public class Star : BaseBody
    {

		[JsonProperty("star_type"), Column("star_type")]
		public StarType StarType { get; private set; }


		/// <summary>
		/// Constructor for EF Core. Not safe to use.
		/// </summary>
		protected Star() : base() { }


		public Star(double radius, StarType type = StarType.MainSequence, string? id = null, string? name = null, string? alias = null, string? description = null) :
			base(radius, id, name ?? "Unnamed Star", alias, description)
		{
			StarType = type;
		}
	}
}
