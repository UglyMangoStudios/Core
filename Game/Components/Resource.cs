using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Components
{

	public enum Group
	{
		Fundamentals,
		Consumables,
		Raw,
		PowerNFuels,
		Fluids,
		Construction,
		Refined,
		SpacetimeExotics,
		Scientific,
		Electromagnetics,
		Elements,
		Ungrouped
	}

	/// <summary>
	/// The basic data structure for game resources. 
	/// </summary>
	[Table("resource")]
	public class Resource
	{
		/// <summary>
		/// The unique identifier of the resource. Used for comparison sake
		/// </summary>
		[JsonProperty("id")]
		[Column("id")]
		[Key]
		public string Id { get; private set; }


		/// <summary>
		/// The default name of this resource. 
		/// If this resource does not have a certain translation, this name will be used.
		/// </summary>
		[JsonProperty("name")]
		[Column("name")]
		public string Name { get; set; }


		/// <summary>
		/// The default description of this resource. 
		/// If this resource does not have a certain translation, this name will be used.
		/// </summary>
		[JsonProperty("description")]
		[Column("description")]
		public string? Description { get; set; }


		/// <summary>
		/// The rarity of the resource
		/// </summary>
		[JsonProperty("rarity")]
		[Column("rarity")]
		public Rarity Rarity { get; set; }


		/// <summary>
		/// The group of the resource
		/// </summary>
		[JsonProperty("group")]
		[Column("group")]
		public Group Group { get; set; }


		/// <summary>
		/// A potential sprite location for the resource. Is null if no path has been provided.
		/// </summary>
		[JsonProperty("image_url")]
		[Column("image_url")]
		public string? ImageUrl { get; set; }


		[JsonProperty("emoji_syntax")]
		[Column("emoji_syntax")]
		public string? EmojiSyntax { get; set; }


		/// <summary>
		/// Create a new game resource internally.
		/// </summary>
		/// <param name="id">The id of the resource</param>
		/// <param name="rarity">The rarity of the resource</param>
		/// <param name="imageLocation">The relative path of the sprite of the resource (optional)</param>
		/// <param name="autoAssignImage">If true, auto assigns a sprite from the provided id that exists in the game resources image dir</param>
		/// <exception cref="ArgumentException">Thrown if a resource with the same id already exists</exception>
		public Resource(string id, string name, Rarity rarity, Group group = Group.Ungrouped, string? description = null, string? imageUrl = null, string? emojiSyntax = null)
		{
			Id = id;
			Name = name;
			Rarity = rarity;
			Group = group;
			Description = description ?? "This resource does not have a description yet.";
			ImageUrl = imageUrl;
			EmojiSyntax = emojiSyntax;
		}

		public static bool operator ==(Resource r, string id) => r.Id == id;
		public static bool operator ==(string id, Resource r) => r.Id == id;
		public static bool operator ==(Resource a, Resource b) => a.Id == b.Id;

		public static bool operator !=(Resource r, string id) => r.Id != id;
		public static bool operator !=(string id, Resource r) => r.Id != id;
		public static bool operator !=(Resource a, Resource b) => a.Id != b.Id;

		public override bool Equals(object? obj) => obj is Resource r && this == r;

		public override int GetHashCode() => Id.GetHashCode();
	}
}
