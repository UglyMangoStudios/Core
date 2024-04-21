using System.Text;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using SpaceCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using SpaceCore.Game.Components;
using SpaceCore.Extensions;
using System.Runtime.Serialization;

namespace SpaceCore.Game.Space.Base
{
	/// <summary>
	/// The basis of all space objects. All sub-classes utilize JSON serialization and deserialization
	/// </summary>

	[JsonObject(MemberSerialization.OptIn)]
    [Table("cosmic_entity")]
	public abstract class CosmicEntity : ITableObject, IFocusableEntity<CosmicEntity>
    {
        protected static readonly Random RANDOM = new();

        /// <summary>
        /// A simple char array pool that is used when creating random IDs
        /// </summary>
        private static readonly char[] ID_CHARS = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuv".ToCharArray();

		/// <summary>
		/// Using a value that usually represents an index, it generates a capitalized character A-Z
		/// </summary>
		/// <param name="index">An int that represents an index</param>
		/// <returns>The made char</returns>
		protected static char _GetIdCharFromIndexCapitalized(int index) => (char)((index % 27) + 65);
		/// <summary>
		/// Using a value that usually represents an index, it generates a lowercased character a-z
		/// </summary>
		/// <param name="index">An int that represents an index</param>
		/// <returns>The made char</returns>
		protected static char _GetIdCharFromIndexLowercased(int index) => (char)((index % 27) + 97);

		/// <summary>
		/// Generates a random ID for this entity if no Id has been set. 
		///
		/// <br/><br/> 
		/// An id is divided into three sections:
		///		[MainSection]:[MidSection]:[TrailingSection]
		///		
		/// <br/><br/> 
		/// An example of a generated id using the default values is "PEN-15" or "Ja9-69"
		/// <br/> 
		/// If middleSectionLength = 4, then an id could look like: "7uW-Pa3D-90"
		/// 
		/// <br/><br/>
		/// The trailing section is always a number that is the length of the parameter. So, if the trailing section
		/// length = 2, then a random number will be generated between 10-100 (inclusive lower bound and exclusive upper bound).
		/// 
		/// </summary>
		/// <param name="mainSectionLen">Number of characters the main section contains (default 3)</param>
		/// <param name="trailingSectionLength">Number of characters the trailing section contains (default 2)</param>
		/// <param name="middleSectionLength"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">When any of the parameters are less than 0</exception>
		protected static string RandomId(int mainSectionLen = 3, int trailingSectionLength = 2, int middleSectionLength = 0)
        {
            if (mainSectionLen < 0 || trailingSectionLength < 0 || middleSectionLength < 0)
                throw new ArgumentException("Cannot have section length less than 0");

            StringBuilder builder = new();

            char __RandomId() => ID_CHARS[RANDOM.Next(ID_CHARS.Length)];

            //Generate id characters for the main section
            for (int i = 0; i < mainSectionLen; i++)
            {
                char idChar = __RandomId();
                while (i - 1 == middleSectionLength && idChar == '-')
                    idChar = __RandomId();

                builder.Append(idChar);
            }
            builder.Append('-'); //The separator character


            //Generate the middle section of the Id if middleSectionLength is greater than 0
            for (int i = 0; i < middleSectionLength; i++)
            {
                char idChar = __RandomId();
                while (i - 1 == middleSectionLength && idChar == '-')
                    idChar = __RandomId();

                builder.Append(idChar);
            }
            if (middleSectionLength != 0) builder.Append('-');

            //Generates the trailing section of the id by generating a number of equal length
            //TODO: Make this more efficient so longer trailing numbers don't exceed the 32-bit size on the int
            int maxNumber = (int)Math.Pow(10, trailingSectionLength);
            int minNumber = (int)Math.Pow(10, trailingSectionLength - 1);

            int trailingNumber = RANDOM.Next(minNumber, maxNumber);
            builder.Append(trailingNumber);

            return builder.ToString();
        }


		/// <summary>
		/// This object's id in the database
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Required]
		public int ObjectId { get; set; }


		/// <summary>
		/// This entity's identifier
		/// </summary>
		[JsonProperty("id")]
        [Column("id")]
        [Required, JsonRequired]
        public string Id { get; internal set; }


        /// <summary>
        /// This entity's name
        /// </summary>
        [JsonProperty("name")]
        [Column("name")]
        public string Name { get; set; }


        /// <summary>
        /// This entity's alias, if it exists
        /// </summary>
        [JsonProperty("alias")]
        [Column("alias")]
        public string? Alias { get; set; }


        /// <summary>
        /// This entity's description. 
        /// 
        /// TODO: Allow for null
        /// </summary>
        [JsonProperty("description")]
        [Column("description")]
        public string? Description { get; set; }


        [JsonProperty("tags")]
        [Column("tags")]
        private List<CosmicTag> _tags { get; set; } = new();
        public ReadOnlyCollection<CosmicTag> CosmicTags => _tags.AsReadOnly();


        [JsonProperty("attributes")]
        [Column("attributes")]
        private List<CosmicAttribute> _attributes { get; set; } = new();
        public ReadOnlyCollection<CosmicAttribute> CosmicAttributes => _attributes.AsReadOnly();


        [JsonProperty("parent")]
        [Column("parent")]
        public virtual CosmicEntity? Parent { get; set; } = null;


        public abstract IReadOnlyList<IFocusable> GetChildren();


        [JsonIgnore]
        public string FocusId => Id;



		/// <summary>
		/// Set the id of this entity. Only internal exposure.
		/// </summary>
		/// <param name="id">The now</param>
		/// <returns>this</returns>
		internal CosmicEntity WithId(string id)
        {
            Id = id;
            return this;
        }

        public CosmicEntity WithTag(CosmicTag tag)
        {
            _tags.Add(tag);
            return this;
        }

        /// <summary>
        /// Assign this entity a description.
        /// </summary>
        /// <param name="d">The value to set</param>
        /// <returns>This object used for method chaining</returns>
        public CosmicEntity WithDescription(string d) { Description = d; return this; }

        /// <summary>
        /// Public constructor to create a space entity
        /// </summary>
        /// <param name="id">The id of the object. If NULL, a random ID will be assigned</param>
        /// <param name="name">The name of the object. If NULL, the ID will be used as the name</param>
        /// <param name="alias">A potential alias for the object.</param>
        /// <param name="description">The description of the object. At this moment, a default non-important description is assigned if NULL</param>
        public CosmicEntity(string? id = null, string? name = null, string? alias = null, string? description = null)
        {
            string randomId = RandomId();

            Id = id ?? randomId;
            Name = name ?? randomId;
            Alias = alias;
            Description = description;
        }


		/// <summary>
		/// Constructor for EF Core. Not safe to use.
		/// </summary>
		protected CosmicEntity() { }



		/// <summary>
		/// Returns a random double between two points
		/// </summary>
		/// <param name="random">The random object use </param>
		/// <param name="min">the minimum number</param>
		/// <param name="max">the maximum number</param>
		/// <returns>The made random double</returns>
		protected static double NextDouble(Random random, double min, double max)
            => random.NextDouble() * (max - min) + min;

        public virtual IFocusable? GetChild(string id) => null;


		[OnDeserialized]
        internal void OnDeserialized(StreamingContext context) => this.AssignSelfToChildren();
    }

    /// <summary>
    /// A collection of attributes that any cosmic entity can possess if they have the <see cref="ICosmicAttribute{TBody}"/> interface
    /// </summary>
    public enum CosmicAttribute
    {
        TimeSquishing, TimeElongation, Strange, NegativeMass, Antimatter, DarkMatterInterference, DarkMatterResonance,
        CosmicRayInterference,
    }


    public enum CosmicTag
    {
        Capital
    }
}
