using Newtonsoft.Json;
using Core.Extensions;
using Core.Game.Components;
using Core.Game.Space;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Data.Saves
{

	[JsonObject(MemberSerialization.OptIn)]
	public class ColonySystems : IFocusable
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, Required]
		public int ObjectId { get; set; }


		[JsonProperty("systems")]
		[Column("systems")]
		public virtual IList<CosmicSystem> Systems { get; set; } = new List<CosmicSystem>(); 

		public string FocusId => "Colony";

		public IFocusable? GetChild(string id) => Systems.FirstOrDefault(s => s.Id == id);

		public IReadOnlyList<IFocusable> GetChildren() => Systems.AsReadOnly();

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context) => this.AssignSelfToChildren();
	}
}
