using Newtonsoft.Json;
using SpaceCore.Game.Components;
using SpaceCore.Game.Entities.Buildables;
using SpaceCore.Game.Space.Base;
using SpaceCore.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Game.Space.Bodies
{
	public enum BodyAttribute
	{
		GoldilocksWorld, VolcanicWorld, IcyWorld, DenseAtmosphere, Surfaceless, Toxic, Sterile, Barren, Desert, OceanWorld, JungleWorld, TundraWorld, MethaneSeas,
		NoMagneticField, WeakMagneticField, StrongMagneticField, HasLiquidWater, HasWater, HasOxygenAtmosphere, HasWeather, TornadoWorld, HurricaneWorld,
		TemperateWorld, WobblyTilt, HasSeasons, AcidRain, PollutedWorld,
	}


	[JsonObject(MemberSerialization.OptIn)]
	[Table("cosmic_body")]
	public abstract class BaseBody : CosmicEntity
	{

		/// <summary>
		/// Constructor for EF Core. Not safe to use.
		/// </summary>
		protected BaseBody(): base() { }


		public BaseBody(double radius, string? id = null, string? name = null, string? alias = null, string? description = null) 
			: base(id, name, alias, description)
		{
			Radius = radius;

			int numRegions = this switch
			{
				Star => 8,
				Planet => 12,
				Moon => 4,
				_ => 0
			};


			//If using regions, this is used to generate the regions
			//for (int i = 0; i < numRegions; i++)
			//{
			//	Region region = new(i);
			//	region.Parent = this;
			//	Regions.Add(region);
			//}
		}


		[JsonProperty("radius")]
		[Column("radius")]
		public double Radius { get; private set; }


		[JsonProperty("body_attributes")]
		[Column("body_attributes")]
		public virtual IList<BodyAttribute> BodyAttributes { get; set; } = new List<BodyAttribute>();


		//When we still used regions
		//[JsonProperty("regions")]
		//[Column("regions")]
		//public virtual IList<Region> Regions { get; set; } = new List<Region>();

		/// <summary>
		/// The dictionary representation of the resources and their corresponding amounts that 
		/// exist within this body
		/// </summary>
		[JsonProperty("harvestables")]
		[Column("harvestables")]
		public ItemQuantityCollection HarvestableResources { get; set; } = [];


		[JsonProperty("buildings")]
		[Column("buildings")]
		public virtual IList<Buildable> Buildings { get; private set; } = new List<Buildable>();


		public BaseBody WithBuilding(Buildable building)
		{
			Buildings.Add(building);
			return this;
		}


		/// <summary>
		/// Adds one or more body attributes to this entity
		/// </summary>
		public BaseBody WithBodyAttribute(BodyAttribute attribute, params BodyAttribute[] attributes)
		{
			attributes.ToList().ForEach(a => WithBodyAttribute(a));
			return WithBodyAttribute(attribute); ;
		}
		/// <summary>
		/// Add a body attribute to this entity
		/// </summary>
		public BaseBody WithBodyAttribute(BodyAttribute attribute)
		{
			BodyAttributes.Add(attribute);
			return this;
		}

		/// <summary>
		/// Adds a consumable resource within this region with its corresponding amount.
		/// If the resource already exists, the value will be added to the existing one.
		/// </summary>
		/// <param name="resource">The resource to add</param>
		/// <param name="amount">The amount to use</param>
		/// <returns>This called object.</returns>
		public BaseBody WithHarvestableResource(Resource resource, ExpoNumber amount)
		{
			if (HarvestableResources.HasItem(resource))
			{
				HarvestableResources[resource] += amount;
				return this;
			}
			HarvestableResources[resource] = amount;
			return this;
		}


		public override IFocusable? GetChild(string id) => 
			GetChildren().FirstOrDefault(child => child.FocusId == id);

		public override IReadOnlyList<IFocusable> GetChildren() => [..Buildings];
	}
}
