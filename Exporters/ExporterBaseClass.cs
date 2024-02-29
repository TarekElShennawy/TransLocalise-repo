using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;
using File = Translator_Project_Management.Models.Database.File;

namespace Translator_Project_Management.Exporters
{
	public abstract class ExporterBaseClass : IExporter, IDisposable
	{
		protected string fileType;

        protected IFileRepository _fileRepository;

        public ExporterBaseClass(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }		

		public bool IsValidExporter(int fileId)
		{
			File dbFile = _fileRepository.GetById(fileId).FirstOrDefault();

			if(dbFile == null || !dbFile.Type.Equals(fileType))
			{
				//This is the invalid exporter as either the file does not exist or the type is not one this exporter
				//is responsible for
				return false;
			}

			return true;
		}

		public abstract string WriteFile(LocFile file);

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
