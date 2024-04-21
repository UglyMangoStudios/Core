
using Newtonsoft.Json;
using SpaceCore.Game.Space;
using SpaceCore.Game.Space.Bodies;
using SpaceCore.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Data.Saves
{

	/// <summary>
	/// A player's game save. 
	/// </summary>
	[Table("player_game_data")]
	public class PlayerGameData
	{
		/// <summary>
		/// The player's user id as it exists in Discord
		/// </summary>
		[JsonProperty("user_id")]
		[Column("user_id")]
		[Key]
		public ulong UserId { get; private set; }


		/// <summary>
		/// This player's seed
		/// </summary>
		[JsonProperty("seed")]
		[Column("seed")]
		public int Seed { get; private set; }


		[JsonIgnore]
		[NotMapped]
		public Random Random { get; }


		//Global resources
		[JsonProperty("credits")]
		[Column("credits")]
		public ExpoNumber Credits { get; set; } = 0;


		[JsonProperty("favor")]
		[Column("favor")]
		public ExpoNumber Favor { get; set; } = 0;


		[JsonProperty("science")]
		[Column("science")]
		public ExpoNumber Science { get; set; } = 0;


		[JsonProperty("home_system", Order = 100)]
		[Column("home_system")]
		public virtual CosmicSystem HomeSystem { get; set; }


		[JsonProperty("built_systems", Order = 101)]
		[Column("built_systems")]
		public virtual IList<CosmicSystem> BuiltSystems { get; } = new List<CosmicSystem>();


		[JsonProperty("colony_systems", Order = 102)]
		[Column("colony_systems")]
		public virtual ColonySystems ColonySystems { get; set; } = new();


		[JsonProperty("outpost_systems", Order = 102)]
		[Column("outpost_systems")]
		public virtual OutpostSystems OutpostSystems { get; set; } = new();


		[JsonConstructor]
		public PlayerGameData(ulong userId) : this(userId, Guid.NewGuid().GetHashCode()) { }
		public PlayerGameData(ulong userId, int seed)
		{
			UserId = userId;
			Seed = seed;
			Random = new Random(Seed);

			HomeSystem = new CosmicSystem(new Star(0));
		}
	}
}
