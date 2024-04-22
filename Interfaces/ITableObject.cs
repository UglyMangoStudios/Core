using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Interfaces
{
	public interface ITableObject
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ObjectId { get; set; }

	}
}
