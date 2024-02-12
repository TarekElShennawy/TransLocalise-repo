namespace Translator_Project_Management.Models.Localisation
{
    public class LocLine
    {
        public string LineId { get; set; }
        public string FileId { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; } = string.Empty;
        public string SourceText { get; set; }
        public string TargetText { get; set; } = string.Empty;
    }
}
