
using Newtonsoft.Json;
using Core.Game.Components;
using Core.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Entities.Buildables.Buildings
{

	public enum BuildingType
	{
		Extractor,
		Refining,
		Organics,
		Physical,
		Computational,
		Manufacturing,
		DeepSpace,
		Science,
		Misc,
	}

	
	[JsonObject(MemberSerialization.OptIn)]
	public class RecipeBuildingFactory : BuildableFactory
	{
		[JsonProperty("recipes")]
		[Column("recipes")]
		public virtual IList<Recipe> Recipes { get; private set; } = new List<Recipe>();


		[JsonProperty("building_type")]
		[Column("building_type")]
		public BuildingType BuildingType { get; private set; }


		// Constructor for Entity Framework. Unsafe to use 
		protected RecipeBuildingFactory() : base() { }

		public RecipeBuildingFactory(string id, string name, 
			ExpoNumber baseEnergyRequirements, string description, 

			Rarity rarity, BuildingType buildingType,
			
			string? imageUrl, string? emoji) : base(id, name, baseEnergyRequirements, rarity, description, imageUrl, emoji)
		{

			BuildingType = buildingType;
		}

		public RecipeBuilding CreateInstance(string buildingId, ExpoNumber? level = null) => new(Id, buildingId, level);

		public override RecipeBuilding CreateInstance(string buildingId) => CreateInstance(buildingId, null);
	}


	[JsonObject(MemberSerialization.OptIn)]
	public class RecipeBuilding(string factoryId, string id, ExpoNumber? level = null) : Buildable(factoryId, id, level)
	{
		[JsonProperty("active_recipe")]
		[Column("active_recipe")]
		
		public virtual IList<Recipe> ActiveRecipes { get; private set; } = new List<Recipe>();
	}
}
