using System.Data.Entity;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class SourceLineRepository : ILineRepository<SourceLine>
	{
		private readonly LocDbContext _dbContext;

		public SourceLineRepository(LocDbContext dbContext)
        {
			_dbContext = dbContext;
        }

		public void Delete(int lineId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<SourceLine> GetAll()
		{
			return _dbContext.SourceLines;
		}

		public Line GetById(int lineId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<SourceLine> GetForFile(int fileId)
		{
			return _dbContext.SourceLines.Where(sl => sl.FileId.Equals(fileId)).Include(sl => sl.File);
		}

		public int Insert(SourceLine line)
		{
            _dbContext.SourceLines.Add(line);
            _dbContext.SaveChanges();
			return line.Id;
        }

		public void Update(SourceLine line)
		{
			throw new NotImplementedException();
		}
	}
}
