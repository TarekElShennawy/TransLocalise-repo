using Translator_Project_Management.Database;
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
		protected ILineRepository _lineRepository;
		protected MySqlDatabase _db;

		public ImporterBaseClass(IFileRepository fileRepository, ILanguageRepository languageRepository, ILineRepository lineRepository, MySqlDatabase db)
		{
			_fileRepository = fileRepository;
			_languageRepository = languageRepository;
			_lineRepository = lineRepository;
			_db = db;
		}

		public abstract LocFile ParseFile(IFormFile file);

		public void Dispose()
		{
			if (_db is IDisposable disposableDb)
			{
				disposableDb.Dispose();
			}

			GC.SuppressFinalize(this);
		}

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
			using (var transaction = _db.Connection.BeginTransaction())
			{
				try
				{
					Models.Database.File dbFile = new Models.Database.File()
					{
						ProjectId = projectId,
						Name = file.Name,
						Type = fileType,
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
