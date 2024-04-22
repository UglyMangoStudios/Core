using Newtonsoft.Json;
using Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Game.Space
{
	//A representation of planetary rings
	[JsonObject(MemberSerialization.OptIn)]
	public class Rings: ITableObject
	{
		/// <summary>
		/// This object's id in the database
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, Required]
		public int ObjectId { get; set; }


	}
}
