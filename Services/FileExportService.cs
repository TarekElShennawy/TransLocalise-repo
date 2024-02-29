using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Exporters;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;
using Translator_Project_Management.Services.Interfaces;
using File = Translator_Project_Management.Models.Database.File;

namespace Translator_Project_Management.Services
{
	public class FileExportService
	{
		private readonly IEnumerable<IExporter> _exporters;
        private readonly IFileRepository _fileRepository;
        private readonly IEmailSender _emailSender;

        private string _fileType;

        public FileExportService(IEnumerable<IExporter> exporters, IFileRepository fileRepository, IEmailSender emailSender)
        {
            _exporters = exporters;
            _fileRepository = fileRepository;
            _emailSender = emailSender;
        }

        public bool ExportAndSendFile(int fileId, string userEmail)
        {
            try
            {
                IExporter exporter = FindSuitableExporter(fileId);

                if(exporter != null)
                {
                    //Get file from DB and parse to List<LocLine> for writing
                    LocFile locFile = ParseFileToLocFile(fileId);

                    if(locFile != null && !string.IsNullOrEmpty(locFile.Name) && locFile.Lines.Any())
                    {
						//Write file content onto a string
						string content = exporter.WriteFile(locFile);

                        if(!string.IsNullOrEmpty(content) &&  !string.IsNullOrEmpty(_fileType))
                        {
                            //Send content as an e-mail attachment
							_emailSender.SendEmail(userEmail, $"{locFile.Name} export", $"Attached file export for: {locFile.Name}", content, locFile.Name, _fileType);

							return true;
						}
					}
                        

                    return false; //No LocFile was created or Loclines are empty
                }

                return false; //No suitable exporter found
            }
            catch
            {
				//Logging exception
				//_logger.LogError(ex.Message);
				return false;
			}
        }

        private LocFile ParseFileToLocFile(int fileId)
        {
			File dbFile = _fileRepository.GetById(fileId)
							.Include(f => f.SourceLines)
							.ThenInclude(sl => sl.Language)
							.Include(f => f.TranslationLines)
							.ThenInclude(tl => tl.Language)
							.FirstOrDefault();

            //Setting fileType for the service to send the correct attachment through email
            _fileType = dbFile.Type;

            LocFile locFile = new LocFile();

            locFile.Name = dbFile.Name;

			foreach (SourceLine line in dbFile.SourceLines)
			{
                TransLine? translation = dbFile.TranslationLines.FirstOrDefault(tl => tl.SourceId.Equals(line.Id));

                LocLine entry = new LocLine()
                {
					LineId = line.LineId,
					SourceText = line.Text,
					SourceLang = line.Language.Code,
                    TargetText = translation?.Text ?? string.Empty,
                    TargetLang = translation?.Language.Code ?? string.Empty
				};				

				locFile.Lines.Add(entry);
			}

            return locFile;
		}

        private IExporter FindSuitableExporter(int fileId)
        {
            //Fetching the correct exporter for the file
            foreach(IExporter exporter in _exporters)
            {
                if(exporter.IsValidExporter(fileId))
                {
                    return exporter;
                }
            }

            return null;
        }
    }
}
