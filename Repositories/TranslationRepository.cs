using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class TranslationRepository : ILineRepository<TransLine>
	{
		private readonly LocDbContext _dbContext;

		public TranslationRepository(LocDbContext dbContext)
        {
			_dbContext = dbContext;
        }

		public void Delete(int lineId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TransLine> GetAll()
		{
            return _dbContext.TranslationLines;
        }

		public TransLine GetById(int lineId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TransLine> GetForFile(int fileId)
		{
			return _dbContext.TranslationLines.Where(tl => tl.FileId.Equals(fileId));
		}

		public int Insert(TransLine translation)
		{
			if(_dbContext.TranslationLines.Any(tl => tl.SourceId.Equals(translation.SourceId)))
			{
				TransLine translationLineToUpdate = _dbContext.TranslationLines.FirstOrDefault(tl => tl.SourceId.Equals(translation.SourceId));
				translationLineToUpdate.Text = translation.Text;
				_dbContext.SaveChanges();

				return translationLineToUpdate.Id;
			}

			_dbContext.TranslationLines.Add(translation);
			_dbContext.SaveChanges();

			return translation.Id;
		}

		public void Update(TransLine line)
		{
			throw new NotImplementedException();
		}
	}
}
