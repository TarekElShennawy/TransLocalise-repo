using Translator_Project_Management.Models.Localisation;

namespace Translator_Project_Management.Importers
{
	public interface IImporter : IDisposable
	{		
		public bool IsValidImporter(IFormFile file);

		public LocFile ParseFile(IFormFile file);

		public void UploadToDb(int projectId, LocFile file);
	}
}
