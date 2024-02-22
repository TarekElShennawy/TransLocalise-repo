using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("lang_code")]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
