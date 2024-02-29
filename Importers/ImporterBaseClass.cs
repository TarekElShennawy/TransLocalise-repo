using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Importers
{
	public abstract class ImporterBaseClass : IImporter, IDisposable
	{
		protected string fileType;

		protected IFileRepository _fileRepository;
		protected ILanguageRepository _languageRepository;
		protected ILineRepository<SourceLine> _sourceLineRepository;
		protected ILineRepository<TransLine> _translationRepository;

		public ImporterBaseClass(IFileRepository fileRepository, ILanguageRepository languageRepository,
			ILineRepository<SourceLine> sourceLineRepository, ILineRepository<TransLine> translationRepository)
		{
			_fileRepository = fileRepository;
			_languageRepository = languageRepository;
			_sourceLineRepository = sourceLineRepository;
			_translationRepository = translationRepository;
		}

		public abstract LocFile ParseFile(IFormFile file);		

		public bool IsValidImporter(IFormFile file)
		{
			if (file == null || !file.FileName.Contains(fileType))
			{
				//file is not valid to be imported by JSON importer as it is null or not a JSON
				return false;
			}

			//If true, parse file
			return true;
		}

		public void UploadToDb(int projectId, LocFile file)
		{
			//Get languages
			List<Language> languages = _languageRepository.GetAll().ToList();
			//Add file entry in files table
			try
			{
				Models.Database.File dbFile = new Models.Database.File()
				{
					ProjectId = projectId,
					Name = file.Name,
					Type = fileType,
					Status = "Not started"
				};

				int fileId = _fileRepository.Insert(dbFile);

				foreach (LocLine line in file.Lines)
				{
					int? sourceLineId = null;

					if (!String.IsNullOrWhiteSpace(line.SourceText) && !String.IsNullOrWhiteSpace(line.SourceLang))
					{
						SourceLine sourceLine = new SourceLine()
						{
							LineId = line.LineId,
							FileId = fileId,
							Text = line.SourceText,
							LangId = languages.Where(l => l.Code.Equals(line.SourceLang)).FirstOrDefault().Id
						};

						sourceLineId = _sourceLineRepository.Insert(sourceLine);
					}

					if (!String.IsNullOrWhiteSpace(line.TargetText) && !String.IsNullOrWhiteSpace(line.TargetLang) && sourceLineId.HasValue)
					{
						TransLine translationLine = new TransLine()
						{
							//TODO: Get Id of the source text created and add it to SourceId property
							SourceId = sourceLineId.Value,
							FileId = fileId,
							Text = line.TargetText,
							LangId = languages.Where(l => l.Code.Equals(line.TargetLang)).FirstOrDefault().Id
						};

						_translationRepository.Insert(translationLine);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
