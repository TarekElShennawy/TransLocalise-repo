namespace Translator_Project_Management.Models.Localisation
{
    public class LocFile
	{
		public string Name { get; set; }

		public List<LocLine> Lines { get; set; } = new List<LocLine>();
	}
}
