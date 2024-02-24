using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

		/// <summary>
		/// Navigation property for the collection of Files belonging to this Project
		/// </summary>
		public virtual ICollection<File> Files { get; set; }

		/// <summary>
		/// Navigation property for the Client relationship
		/// </summary>
		[ForeignKey("ClientId")]
		public virtual Client Client { get; set; }

        [Required]
        [Column("client_id")]        
        public int ClientId { get; set; }

		/// <summary>
		/// Navigation property for the Manager (of type User) relationship
		/// </summary>
		[ForeignKey("ManagerId")]
		public virtual User Manager { get; set; }
       
        [Required]
        [Column("manager_id")]        
        public string ManagerId { get; set; }

        [Column("start_date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Column("due_date")]
        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public string Status { get; set; }

        public string Description { get; set; } = String.Empty;
    }
}
