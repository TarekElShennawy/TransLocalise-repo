using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class LanguageRepository : ILanguageRepository
	{
		private readonly LocDbContext _dbContext;

		public LanguageRepository(LocDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void Delete(int langId)
		{
			Language lang = _dbContext.Languages.Find(langId);

			if (lang != null)
			{
				_dbContext.Languages.Remove(lang);
			}
		}

		public IEnumerable<Language> GetAll()
		{
			return _dbContext.Languages;
		}

		public Language GetById(int langId)
		{
			return _dbContext.Languages.Find(langId);
		}

		public void Insert(Language language)
		{
			_dbContext.Languages.Add(language);
			_dbContext.SaveChanges();
		}

		public void Update(Language language)
		{
			_dbContext.Update(language);
			_dbContext.SaveChanges();
		}
	}
}
