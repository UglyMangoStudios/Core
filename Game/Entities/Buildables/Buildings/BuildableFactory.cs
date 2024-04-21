using Newtonsoft.Json;
using SpaceCore.Game.Components;
using SpaceCore.Interfaces;
using SpaceCore.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Game.Entities.Buildables.Buildings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class BuildableFactory : ITableObject
	{
		/// <summary>
		/// This object's id in the database
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, Required]
		public int ObjectId { get; set; }


		[JsonProperty("id")]
		[Column("id")]
		public string Id { get; private set; }


		[JsonProperty("name")]
		[Column("name")]
		public string Name { get; private set; }


		[JsonProperty("description")]
		[Column("description")]
		public string? Description { get; private set; }


		[JsonProperty("base_energy_cost")]
		[Column("base_energy_cost")]
		public ExpoNumber BaseEnergyCost { get; private set; }


		[JsonProperty("image_url")]
		[Column("image_url")]
		public string? ImageUrl { get; private set; }


		[JsonProperty("emoji")]
		[Column("emoji")]
		public string? Emoji { get; private set; }


		[JsonProperty("rarity")]
		[Column("rarity")]
		public Rarity Rarity { get; private set; }


		//Used for Entity Framework
		protected BuildableFactory() { }


		public BuildableFactory(string id, string name, ExpoNumber baseEnergyCost, Rarity rarity, string description, string? imageUrl, string? emoji)
		{
			Id = id;
			Name = name;
			Description = description;
			ImageUrl = imageUrl;
			Emoji = emoji;

			Rarity = rarity;

			BaseEnergyCost = baseEnergyCost;
		}

		/// <summary>
		/// The default implementation of creating an instance of the object this schema represents
		/// </summary>
		public virtual Buildable CreateInstance(string buildingId) => throw new NotImplementedException();
	}
}
