using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
	public class UserSourceLineMapping
	{
		[Key]
		public int Id { get; set; }

		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; } //Navigation property to User

		public int SourceLineId { get; set; }

		[ForeignKey("SourceLineId")]
		public SourceLine SourceLine { get; set; } //Navigation property to Source Line
	}
}
