namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface IFileRepository
	{
		//Gets all files
		IEnumerable<Models.Database.File> GetAll();

        //Gets files by project ID
        IEnumerable<Models.Database.File> GetByProjectId(int projectId);

		//Inserts a new file record and returns the FileId
		int Insert(Models.Database.File file);

		//Updates a file record
		void Update(Models.Database.File file);

		//Deletes a file record
		void Delete(int fileId);
	}
}
