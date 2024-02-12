using Translator_Project_Management.Models.Localisation;

namespace Translator_Project_Management.Importers
{
	interface IImporter
	{		
		public bool IsValidImporter(IFormFile file);

		public void UploadToDb(int projectId, LocFile file);
	}
}
