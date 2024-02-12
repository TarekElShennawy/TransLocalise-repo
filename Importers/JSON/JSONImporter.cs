using System.IO;
using Translator_Project_Management.Models.Localisation;

namespace Translator_Project_Management.Importers.XML
{
	public class JSONImporter : IImporter
	{

		private string content;

		private string fileType = "json";

		public bool IsValidImporter(IFormFile file)
		{
			if (file == null)
			{
				//file is not valid to be imported by JSON importer as it is null or not a JSON
				return false;
			}

			//Check if the file-type is JSON

			//If true, parse file
			return true;
		}

		public void ParseFile(string content)
		{
			//Go through the file's lines

			//After lines are parsed, upload to DB
		}

		public void UploadToDb(int projectId, LocFile file)
		{
			//Check if database contains line

			//If not, add the line's text to the DB with the associated file's ID and language ID
		}

	}
}