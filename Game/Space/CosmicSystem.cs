using Newtonsoft.Json;
using Core.Game.Components;
using Core.Game.Space.Base;
using Core.Game.Space.Bodies;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Space
{

	/// <summary>
	/// The basis of every collection of space entities. 
	/// </summary>
	/// <typeparam name="TCenter">The type that represents the host/center of each system. 
	/// This object must inherit from <see cref="CosmicEntity"/></typeparam>
	[JsonObject(MemberSerialization.OptIn)]
	[Table("cosmic_system")]
	public class CosmicSystem : CosmicEntity
	{

		/// <summary>
		/// The center of the system
		/// </summary>
		[JsonProperty("center")]
		[Column("center")]
		public virtual BaseBody SystemCenter { get; private set; }


		/// <summary>
		/// Additional objects that exist in the center. E.g. binary star systems
		/// </summary>
		[JsonProperty("center_siblings")]
		[Column("center_siblings")]
		public virtual IList<BaseBody> CenterSiblings { get; } = new List<BaseBody>();


		/// <summary>
		/// Represents the objects that orbit the center
		/// </summary>
		[JsonProperty("orbiters")]
		[Column("orbiters")]
		public virtual IList<CosmicEntity> Orbiters { get; } = new List<CosmicEntity>();


		[JsonProperty("neighboring_system")]
		[Column("neighboring_system")]
		public virtual IList<CosmicSystem> NeighboringSystem { get; } = new List<CosmicSystem>();


		public virtual SystemState State { get; set; } = SystemState.Undiscovered;


		/// <summary>
		/// Simple constructor for creating systems
		/// </summary>
		/// <param name="systemCenter">The center of this system</param>
		/// <param name="id">The id of this system (optional). If null, a random id will be generated</param>
		/// <param name="name">The name of this system (optional). If null, the id of this system will be used to name it</param>
		/// <param name="alias">Any alias of this system (optional).</param>
		/// <param name="description">Any description for this system (optional).</param>
		/// <exception cref="ArgumentException"/>
		public CosmicSystem(BaseBody systemCenter, string? id = null, string? name = null, string? alias = null, string? description = null)
			: base(id, name, alias, description)
		{
			//Ensure that the system center is not another system
			if (systemCenter.GetType() == typeof(CosmicSystem))
				throw new ArgumentException("Cannot have a system be the center of another system: " + nameof(systemCenter));

			SystemCenter = systemCenter;
			SystemCenter.Parent = this;
			systemCenter.WithId(Id + "A");
		}


		/// <summary>
		/// Constructor for EF Core. Not safe to use.
		/// </summary>
		protected CosmicSystem() : base() { }

		/// <summary>
		/// Adds additional objects as this star system's center to act as host or center siblings
		/// </summary>
		/// <returns>This object used for method chaining</returns>
		public CosmicSystem WithSibling(BaseBody sibling, params BaseBody[] siblings)
		{
			siblings.ToList().ForEach(s => WithSibling(s));
			return WithSibling(sibling);
		}

		public override IFocusable? GetChild(string id)
		{
			if (id == SystemCenter.Id) return SystemCenter;
			return CenterSiblings.FirstOrDefault(c => c.Id == id) ?? Orbiters.FirstOrDefault(o => o.Id == id);
		}

		/// <summary>
		/// Add an additional object as this star system's center to act as host or center siblings
		/// </summary>
		/// <returns>This object used for method chaining</returns>
		public CosmicSystem WithSibling(BaseBody sibling)
		{
			string newId = Id + _GetIdCharFromIndexCapitalized(CenterSiblings.Count + 1);
			sibling.WithId(newId);

			CenterSiblings.Add(sibling);
			sibling.Parent = this;
			return this;
		}


		/// <summary>
		/// Adds <see cref="CosmicEntity"/> objects that "orbit" this system
		/// </summary>
		/// <returns>This object used for method chaining</returns>
		public CosmicSystem WithOrbiter(CosmicEntity entity, params CosmicEntity[] orbiters) => WithOrbiter(entity, true, orbiters);


		/// <summary>
		/// Adds <see cref="CosmicEntity"/> objects that "orbit" this system
		/// </summary>
		/// <param name="overwriteId">Default=true. If TRUE, the entity's id will be overwritten to match this system's id and this entity's index in orbit</param>
		/// <returns>This object used for method chaining</returns>
		public CosmicSystem WithOrbiter(CosmicEntity entity, bool overwriteId = true, params CosmicEntity[] orbiters)
		{
			orbiters.ToList().ForEach(e => WithOrbiter(e, overwriteId));
			return WithOrbiter(entity, overwriteId);
		}


		/// <summary>
		/// Adds a <see cref="CosmicEntity"/> object that "orbits" this system
		/// </summary>
		/// <param name="overwriteId">Default=true. If TRUE, the entity's id will be overwritten to match this system's id and this entity's index in orbit</param>
		/// <returns>This object used for method chaining</returns>
		public CosmicSystem WithOrbiter(CosmicEntity entity, bool overwriteId = true)
		{
			string newId = Id + _GetIdCharFromIndexLowercased(Orbiters.Count);
			if(overwriteId) entity.WithId(newId);

			Orbiters.Add(entity);
			entity.Parent = this;

			return this;
		}

		public CosmicSystem WithNeighboringSystem<TNeighbor>(CosmicSystem neighbor) where TNeighbor : BaseBody
		{
			this.NeighboringSystem.Add(neighbor);
			neighbor.NeighboringSystem.Add(this);
			return this;
		}

		public override IReadOnlyList<IFocusable> GetChildren() =>
			[SystemCenter, ..CenterSiblings, ..Orbiters, ..NeighboringSystem];
	}


	public enum SystemState
	{
		Undiscovered, 
		Discovered,
		Outposted,
		Colonized,
		Assimilated,
	}

}
