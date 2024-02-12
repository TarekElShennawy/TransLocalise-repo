using MySqlConnector;
using System.Transactions;
using System.Web.WebPages;
using System.Xml;
using System.Xml.Linq;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Importers.XML
{
	public class XLIFFImporter : IImporter
	{
		private string fileType = "xliff";	

		private IFileRepository _fileRepository;
		private ILanguageRepository _languageRepository;
		private ILineRepository _lineRepository;
		private MySqlDatabase _db;

        public XLIFFImporter(IFileRepository fileRepository, ILanguageRepository languageRepository, ILineRepository lineRepository, MySqlDatabase db)
        {
			_fileRepository = fileRepository;
			_languageRepository = languageRepository;
			_lineRepository = lineRepository;
			_db = db;
		}

        public bool IsValidImporter(IFormFile file)
		{
			if (file == null || !file.FileName.Contains(fileType))
			{
				//File is not XLIFF
				return false;
			}

			return true;				
		}

		public LocFile ParseFile(IFormFile file)
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
							xliffFile.Name = fileElement.Attribute("original")?.Value;

							//Parsing unit data
							foreach (XElement unitElement in doc.Descendants("unit"))
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
			catch (XmlException exception)
			{
				//Error with XML, probably incorrect.
			}

			//After the file is parsed, upload to DB
			return xliffFile;
		}

		public void UploadToDb(int projectId, LocFile file)
		{
			//Get languages
			List<Language> languages = _languageRepository.GetAll().ToList();
			//Add file entry in files table
			using(var transaction = _db.Connection.BeginTransaction())
			{
				try
				{
					Models.Database.File dbFile = new Models.Database.File()
					{
						ProjectId = projectId,
						Name = file.Name,
						Type = "Xliff",
						Status = "Not started"
					};

					int fileId = _fileRepository.Insert(dbFile, transaction);

					foreach (LocLine line in file.Lines)
					{
						if (!String.IsNullOrWhiteSpace(line.SourceText) && !String.IsNullOrWhiteSpace(line.SourceLang))
						{
							Line sourceLine = new Line()
							{
								LineId = line.LineId,
								FileId = fileId,
								Text = line.SourceText,
								LangId = languages.Where(l => l.Code.Equals(line.SourceLang)).FirstOrDefault().Id,
								IsTranslation = false
							};

							_lineRepository.Insert(sourceLine, transaction);
						}

						if (!String.IsNullOrWhiteSpace(line.TargetText) && !String.IsNullOrWhiteSpace(line.TargetLang))
						{
							Line translationLine = new Line()
							{
								LineId = line.LineId,
								FileId = fileId,
								Text = line.TargetText,
								LangId = languages.Where(l => l.Code.Equals(line.TargetLang)).FirstOrDefault().Id,
								IsTranslation = true
							};

							_lineRepository.Insert(translationLine, transaction);
						}						
					}

					// Committing all inserts at once after the loop
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;					
				}
			}
		}
	}
}