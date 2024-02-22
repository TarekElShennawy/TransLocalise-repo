using System.Data.Entity;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;
using File = Translator_Project_Management.Models.Database.File;

namespace Translator_Project_Management.Repositories
{
	public class FileRepository : IFileRepository
	{
		private readonly LocDbContext _dbContext;

		public FileRepository(LocDbContext dbContext)
        {
			_dbContext = dbContext;
		}

        public void Delete(int fileId)
		{
			var file = _dbContext.Files.Find(fileId);
			if(file != null)
			{
				_dbContext.Files.Remove(file);
                _dbContext.SaveChanges();
            }
		}

		public IEnumerable<Models.Database.File> GetAll()
		{
			return _dbContext.Files;
		}

		public IEnumerable<Models.Database.File> GetByProjectId(int projectId)
		{
			return _dbContext.Files.Where(f => f.ProjectId.Equals(projectId));
		}

		public int Insert(Models.Database.File file)
		{
			_dbContext.Files.Add(file);
			_dbContext.SaveChanges();

			return file.Id;
		}

		public void Update(Models.Database.File file)
		{
			_dbContext.Files.Update(file);
			_dbContext.SaveChanges();
		}
	}
}
