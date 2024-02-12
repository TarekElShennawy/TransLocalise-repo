namespace Translator_Project_Management.Models.Database
{
	public class Line
	{
		public string LineId { get; set; }
		public int FileId { get; set; }
		public string Text { get; set; }
		public int LangId { get; set; }
		public bool IsTranslation { get; set; }
	}
}
