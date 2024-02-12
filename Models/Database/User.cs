namespace Translator_Project_Management.Models.Database
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int LanguageId { get; set; }
        public string SkillsAndExperience { get; set; }

    }
}
