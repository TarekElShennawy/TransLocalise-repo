using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    public class Line
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Navigation property for the File this Line belongs to
        /// </summary>
        [ForeignKey("FileId")]
		public virtual File File { get; set; }

		[Required]
        [Column("file_id")]
        public int FileId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [Column("lang_id")]
        public int LangId { get; set; }

        // Navigation property for Language
        [ForeignKey("LangId")]
		public virtual Language Language { get; set; }
	}
}
