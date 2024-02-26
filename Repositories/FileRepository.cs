using Microsoft.EntityFrameworkCore;
using System.Linq;
using Translator_Project_Management.Database;
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

		public IQueryable<File> GetAll()
		{
			return _dbContext.Files.Include(f => f.SourceLines);
		}

		public IQueryable<File> GetById(int fileId)
		{
			return _dbContext.Files.Where(f => f.Id.Equals(fileId));
		}

		public int Insert(File file)
		{
			if(!_dbContext.Files.Any(f => f.Name.Equals(file.Name) && f.Type.Equals(file.Type))) //Checking whether a file with the same name and type is in the DB already.
			{
				_dbContext.Files.Add(file);
				_dbContext.SaveChanges();

				return file.Id;
			}
				//The method returns the file ID and we know the file exists, therefore return the Id of the file that is already in DB
				return _dbContext.Files.FirstOrDefault(f => f.Name.Equals(file.Name) && f.Type.Equals(file.Type)).Id;
			}

		public void Update(Models.Database.File file)
		{
			_dbContext.Files.Update(file);
			_dbContext.SaveChanges();
		}
	}
}
