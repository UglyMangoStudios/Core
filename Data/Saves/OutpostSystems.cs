using Newtonsoft.Json;
using SpaceCore.Extensions;
using SpaceCore.Game.Components;
using SpaceCore.Game.Space;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpaceCore.Data.Saves
{

	[JsonObject(MemberSerialization.OptIn)]
	public class OutpostSystems : IFocusable
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, Required]
		public int ObjectId { get; set; }


		[JsonProperty("systems")]
		[Column("systems")]
		public virtual IList<CosmicSystem> Systems { get; set; } = new List<CosmicSystem>();

		public string FocusId => "Outpost";

		public IFocusable? GetChild(string id) => Systems.FirstOrDefault(s => s.Id == id);

		public IReadOnlyList<IFocusable> GetChildren() => Systems.AsReadOnly();


		[OnDeserialized]
		private void OnDeserialized(StreamingContext context) => this.AssignSelfToChildren();
	}
}
