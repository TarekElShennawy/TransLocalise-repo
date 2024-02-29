using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Exporters.JSON
{
	public class JSONExporter : ExporterBaseClass
	{
        public JSONExporter(IFileRepository fileRepository)
            : base(fileRepository)
        {
			fileType = "json";
        }

		public override string WriteFile(LocFile file)
		{
			string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(file.Lines);

			return jsonContent;
		}
	}
}
