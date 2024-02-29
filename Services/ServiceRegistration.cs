using Translator_Project_Management.Exporters.JSON;
using Translator_Project_Management.Exporters.XLIFF;
using Translator_Project_Management.Exporters;
using Translator_Project_Management.Importers.JSON;
using Translator_Project_Management.Importers.XML;
using Translator_Project_Management.Importers;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;
using Translator_Project_Management.Repositories;

namespace Translator_Project_Management.Services
{
	public static class ServiceRegistration
	{
		public static void AddServices(IServiceCollection services)
		{
			//Repository services
			services.AddTransient<IProjectRepository, ProjectRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IClientRepository, ClientRepository>();
			services.AddTransient<IFileRepository, FileRepository>();
			services.AddTransient<ILanguageRepository, LanguageRepository>();
			services.AddTransient<ILineRepository<SourceLine>, SourceLineRepository>();
			services.AddTransient<ILineRepository<TransLine>, TranslationRepository>();
			services.AddTransient<IUserSourceLineMappingRepository, UserSourceLineMappingRepository>();

			//Importer services
			services.AddTransient<IImporter, XLIFFImporter>();
			services.AddTransient<IImporter, JSONImporter>();

			//Exporter services
			services.AddTransient<IExporter, JSONExporter>();
			services.AddTransient<IExporter, XliffExporter>();

			//File import and export
			services.AddTransient<FileImportService>();
			services.AddTransient<FileExportService>();
		}
	}
}