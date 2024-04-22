using Newtonsoft.Json;
using Core.Extensions;
using Core.Game.Components;
using Core.Game.Space.Bodies;
using Core.Interfaces;
using Core.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Game.Entities.Buildables
{

	[JsonObject(MemberSerialization.OptIn)]
	[Table("building")]
	public abstract class Buildable: ITableObject, IFocusableEntity<BaseBody>
	{
		/// <summary>
		/// This object's id in the database
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, JsonRequired, Required]
		public int ObjectId { get; set; }


		[JsonProperty("id")]
		[Column("id")]
		public string Id { get; private set; }


		[JsonProperty("name")]
		[Column("name")]
		public string Name { get; set; }


		[JsonProperty("factory_id")]
		[Column("factory_id")]
		public string FactoryId { get; private set; }


		[JsonProperty("level")]
		[Column("level")]
		public ExpoNumber Level { get; set; } = 0;


		[JsonProperty("parent")]
		[Column("parent")]
		public virtual BaseBody? Parent { get; set; }


		[JsonConstructor]
		public Buildable(string factoryId, string id, ExpoNumber? level = null, string? name = null)
		{
			Id = id;
			FactoryId = factoryId;
			Level = level ?? 1;
			Name = name ?? "Unnamed Building";
		}

		public virtual IFocusable? GetChild(string id) => null;

		public virtual IReadOnlyList<IFocusable> GetChildren() => [];

		public string FocusId => Id;


		[OnDeserialized]
		internal void OnDeserialized(StreamingContext context) => this.AssignSelfToChildren();
	}
}
