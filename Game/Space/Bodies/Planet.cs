using Newtonsoft.Json;
using Core.Game.Components;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Space.Bodies
{
	/// <summary>
	/// Represents a planetary object. Most of the player game data will live in the planets they colonize
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
    [Table("planet")]
	public class Planet : BaseBody
    {

        public static Planet AsRocky(string? id = null, string? name = null, string? alias = null, string? description = null)
        {
            const double minRadius = .95;
            const double maxRadius = 1.05;

            Planet planet = new(1, id, name, alias, description);

            return planet;
		}

		public static Planet AsGasGiant(string? id = null, string? name = null, string? alias = null, string? description = null)
		{
			const double minRadius = .95;
			const double maxRadius = 1.05;

			Planet planet = new(1, id, name, alias, description);
			planet.WithBodyAttribute(BodyAttribute.DenseAtmosphere, BodyAttribute.Surfaceless);

			return planet;
		}

		public static Planet AsIceGiant(string? id = null, string? name = null, string? alias = null, string? description = null)
		{
			const double minRadius = .95;
			const double maxRadius = 1.05;

			Planet planet = new(1, id, name, alias, description);
			planet.WithBodyAttribute(BodyAttribute.DenseAtmosphere, BodyAttribute.Surfaceless, BodyAttribute.IcyWorld);

			return planet;
		}

		/// <summary>
		/// A collection of this planet's moons
		/// </summary>
		[JsonProperty("moons")]
        [Column("moons")]
        public virtual IList<Moon> Moons { get; } = new List<Moon>();


        /// <summary>
        /// A nullable field that represents this planet's rings
        /// </summary>
        [JsonProperty("rings")]
        [Column("rings")]
        public virtual Rings? Rings { get; set; } = null;

		/// <summary>
		/// Constructor for EF Core. Not safe to use.
		/// </summary>
		protected Planet() : base() { }

		public Planet(double radius, string? id = null, string? name = null, string? alias = null, string? description = null) :
            base(radius, id, name ?? "Unnamed Planet", alias, description)
        {

		}


        /// <summary>
        /// Adds moons that will reside in this planet's orbit
        /// </summary>
        /// <returns>This object for method chaining</returns>
        public Planet WithMoon(Moon moon, params Moon[] moons)
        {
            moons.ToList().ForEach(m => WithMoon(m));
            return WithMoon(moon);
        }

        /// <summary>
        /// Add a moon that will reside in this planet's orbit
        /// </summary>
        /// <returns>This object for method chaining</returns>
        public Planet WithMoon(Moon moon)
        {
            moon.Id = Id + "-" + (Moons.Count + 1);
            Moons.Add(moon);
            return this;
        }

        /// <summary>
        /// Add a ring system that will reside in this planet's orbit
        /// </summary>
        /// <param name="rings">The ring to add</param>
        /// <returns>This object for method chaining</returns>
        public Planet WithRings(Rings rings)
        {
            Rings = rings;
            return this;
        }

		public override IFocusable? GetChild(string id)
		{
			return Moons.FirstOrDefault(m => m.Id == id) ?? base.GetChild(id);
		}

        public override IReadOnlyList<IFocusable> GetChildren() => [.. Moons, .. base.GetChildren()];
	}
}
