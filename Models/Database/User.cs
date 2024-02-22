using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    public class User : IdentityUser
    {
        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

		[ForeignKey("LanguageId")]
		public virtual Language Language { get; set; }

        /// <summary>
        /// Navigation property for the Language relationship
        /// </summary>
        [Required]        
        [Column("language")]
        public int LanguageId { get; set; }

        [Column("skills_and_experience")]
        public string SkillsAndExperience { get; set; }

		public ICollection<UserSourceLineMapping> UserSourceLineMappings { get; set; } = new List<UserSourceLineMapping>();
	}
}
