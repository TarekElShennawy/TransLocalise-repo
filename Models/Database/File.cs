using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    public class File
    {
        [Key]
        public int Id { get; set; }

		/// <summary>
		/// Navigation property to the project that belongs to a file
		/// </summary>
		[ForeignKey("ProjectId")]
		public virtual Project Project { get; set; }

        [Required]
        [Column("project_id")]        
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Status { get; set; }

		public virtual ICollection<SourceLine> SourceLines { get; set; }
		public virtual ICollection<TransLine> TranslationLines { get; set; }
	}
}
