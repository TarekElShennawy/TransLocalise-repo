using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface ILanguageRepository
	{
		//Get all languages
		IEnumerable<Language> GetAll();

		//Get language by ID
		Language GetById(int langId);

		//Inserts a new language record
		void Insert(Language language);

		//Update a language
		void Update(Language language);

		//Deletes a language
		void Delete(int langId);
	}
}
