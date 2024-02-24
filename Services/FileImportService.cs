using Translator_Project_Management.Importers;
using Translator_Project_Management.Models.Localisation;

namespace Translator_Project_Management.Services
{
	public class FileImportService
	{
		private readonly IEnumerable<IImporter> _importers;
        //private readonly ILogger _logger;

        public FileImportService(IEnumerable<IImporter> importers)
        {
            _importers = importers;
            //_logger = logger;
        }

        public bool AddFile(IFormFile file, int projectId)
        {
            try
            {
                //Find suitable importer
                IImporter importer = FindSuitableImporter(file);

                if(importer != null)
                {
                    //Parse file and upload to DB
                    LocFile parsedFile = importer.ParseFile(file);
                    importer.UploadToDb(projectId, parsedFile);

                    return true; //Indicate success
                }
                else
                {
                    return false; //No suitable importer found
                }
            }
            catch (Exception ex)
            {
				//Logging exception
				//_logger.LogError(ex.Message);
				return false;
            }
        }

        private IImporter FindSuitableImporter(IFormFile file)
        {
            //Fetching the correct importer for the file type
            foreach(IImporter importer in _importers)
            {
                if(importer.IsValidImporter(file))
                {
                    return importer;
                }
            }

            return null;
        }
    }
}
