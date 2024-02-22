using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    [Table("translations")]
    public class TransLine : Line
    {
        [Required]
        [Column("source_id")]
        public int SourceId { get; set; }
    }
}
