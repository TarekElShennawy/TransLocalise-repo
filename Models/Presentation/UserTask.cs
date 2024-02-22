namespace Translator_Project_Management.Models.Presentation
{
    public class UserTask
    {
        //The text tasked to be translated by the user
        public string Source { get; set; }

        //The ID that belongs to the source text for setting the translation in DB
        public int SourceId { get; set; }

        //The translation inserted by the user
        public string Translation { get; set; } = string.Empty;

        //The ID of the file the task belongs to
        public int FileId { get; set; }
    }
}
