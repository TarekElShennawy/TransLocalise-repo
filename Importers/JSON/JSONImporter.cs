using Newtonsoft.Json;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Importers.JSON
{
	public class JSONImporter : ImporterBaseClass
	{
		public JSONImporter(IFileRepository fileRepository, ILanguageRepository languageRepository,
			ILineRepository<SourceLine> sourceLineRepository, ILineRepository<TransLine> translationRepository)
		: base(fileRepository, languageRepository, sourceLineRepository, translationRepository)
		{
			_fileRepository = fileRepository;
			_languageRepository = languageRepository;
			_sourceLineRepository = sourceLineRepository;
			_translationRepository = translationRepository;

			fileType = "json";
		}

		public override LocFile ParseFile(IFormFile file)
		{
			LocFile jsonFile = new LocFile();

			jsonFile.Name = file.FileName.Split('.')[0];
			//Go through the file's lines
			using (var stream = file.OpenReadStream())
			{
				using (var reader = new StreamReader(stream))
				{
					string fileContents = reader.ReadToEnd();

					if(!string.IsNullOrWhiteSpace(fileContents))
					{
						List<LocLine> locLines = JsonConvert.DeserializeObject<List<LocLine>>(fileContents);

						if(locLines != null && locLines.Any())
						{
							foreach (LocLine line in locLines)
							{
								jsonFile.Lines.Add(line);
							}
						}						
					}					
				}
			}

			return jsonFile;
		}
	}
}