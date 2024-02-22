using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator_Project_Management.Models.Database
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column("contact_name")]
        public string ContactName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("billing_address")]
        public string BillingAddress { get; set; }
    }
}
