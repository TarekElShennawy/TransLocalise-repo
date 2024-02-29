using System.Xml;
using System.Xml.Linq;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Importers.XML
{
	public class XLIFFImporter : ImporterBaseClass
	{
		public XLIFFImporter(IFileRepository fileRepository, ILanguageRepository languageRepository,
			ILineRepository<SourceLine> sourceLineRepository, ILineRepository<TransLine> translationRepository)
		: base(fileRepository, languageRepository, sourceLineRepository, translationRepository)
		{
			_fileRepository = fileRepository;
			_languageRepository = languageRepository;
			_sourceLineRepository = sourceLineRepository;
			_translationRepository = translationRepository;

			fileType = "xliff";
		}

		public override LocFile ParseFile(IFormFile file)
		{
			LocFile xliffFile = new LocFile();

			//Go through the file's lines
			try
			{
				using (Stream stream = file.OpenReadStream())
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						stream.CopyTo(memoryStream);
						memoryStream.Position = 0;					

						// Use the XDocument object to parse the XML content
						XDocument doc = XDocument.Load(memoryStream);						

						//Parsing file metadata
						foreach (XElement fileElement in doc.Descendants("file"))
						{
							//Setting the file name based on either the file name attribute in the file element or the actual file's name
							string[] xliffFileName = fileElement.Attribute("original")?.Value.Split('.');

							if (!string.IsNullOrWhiteSpace(xliffFileName[0]))
							{
								xliffFile.Name = xliffFileName[0];
							}
							else
							{
								xliffFile.Name = file.FileName.Split('.')[0];
							}

							IEnumerable<XElement> units = doc.Descendants("unit");

							if(units != null && units.Any())
							{
								//Parsing unit data
								foreach (XElement unitElement in units)
								{
									LocLine line = new LocLine()
									{
										LineId = unitElement.Attribute("id")?.Value,
										SourceLang = fileElement.Attribute("source-language")?.Value,
										TargetLang = fileElement.Attribute("target-language")?.Value,
										SourceText = unitElement.Element("source")?.Value,
										TargetText = unitElement.Element("target")?.Value
									};

									xliffFile.Lines.Add(line);
								}
							}							
						}						
					}
				}
			}
			catch (XmlException exception)
			{
				//Error with XML, probably incorrect.
			}

			//After the file is parsed, upload to DB
			return xliffFile;
		}
	}
}