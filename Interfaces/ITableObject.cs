using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Interfaces
{
	public interface ITableObject
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ObjectId { get; set; }

	}
}
