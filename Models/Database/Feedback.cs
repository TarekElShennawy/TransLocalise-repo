namespace Translator_Project_Management.Models.Database
{
    public class Feedback
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int TranslatorId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
