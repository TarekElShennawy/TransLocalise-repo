using File = Translator_Project_Management.Models.Database.File;

namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface IFileRepository
	{
		//Gets all files
		IQueryable<File> GetAll();

		//Gets file by ID
		IQueryable<File> GetById(int fileId);

		//Inserts a new file record and returns the FileId
		int Insert(File file);

		//Updates a file record
		void Update(File file);

		//Deletes a file record
		void Delete(int fileId);
	}
}
