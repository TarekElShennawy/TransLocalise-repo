using Translator_Project_Management.Models.Localisation;

namespace Translator_Project_Management.Exporters
{
	public interface IExporter : IDisposable
	{
		public bool IsValidExporter(int fileId);
		public string WriteFile(LocFile locFile);
	}
}
