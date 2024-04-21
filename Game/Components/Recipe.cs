using Newtonsoft.Json;
using SpaceCore.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Game.Components
{

    /// <summary>
    /// Representation of a resource recipe with inputs (optional) and outputs. 
    /// </summary>
    [Table("recipe")]
    public class Recipe
    {
        /// <summary>
        /// The unique identifier of this recipe
        /// </summary>
        [Key]
        [JsonProperty("id")]
        [Column("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The screenname of this recipe
        /// </summary>
        [JsonProperty("name")]
        [Column("name")]
        public string Name { get; private set; }


        /// <summary>
        /// The description of this recipe
        /// </summary>
        [JsonProperty("description")]
        [Column("description")]
        public string Description { get; private set; }

        /// <summary>
        /// The amount of time of this recipe. NOTE: this variable and its usage may change once the algorithms for recipe usages get ironed out. 
        /// </summary>
        [Obsolete]
        public uint Time { get; private set; }

        /// <summary>
        /// The immutable input values of this recipe
        /// </summary>
        [JsonProperty("input")]
        [Column("input")]
		public ItemQuantityCollection Input { get; set; }

        /// <summary>
        /// The immutable output values of this recipe
        /// </summary>
        [JsonProperty("output")]
        [Column("output")]
        public ItemQuantityCollection Output { get; private set; }

        public Recipe(string id, string name, uint time, ItemQuantityCollection input, ItemQuantityCollection output, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description ?? "This recipe does not yet contain a description.";
            Time = time;


            Input = input;
            Output = output;
        }
	}
}
