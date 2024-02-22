using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    [Table("source_text")]
    public class SourceLine : Line
	{
		[Required]
		[Column("line_id")]
		public string LineId { get; set; }

		public ICollection<UserSourceLineMapping> UserSourceLineMappings { get; set; }
	}
}
