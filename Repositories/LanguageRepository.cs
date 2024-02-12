using MySqlConnector;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class LanguageRepository : ILanguageRepository
	{
		private string c_GetAllLangsCmd = "SELECT * FROM languages";

		private readonly MySqlDatabase _db;
		public LanguageRepository(MySqlDatabase db)
		{
			_db = db;
		}

		public void Delete(int langId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Language> GetAll()
		{
			List<Language> languages = new List<Language>();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetAllLangsCmd;

			using (MySqlDataReader reader = cmd.ExecuteReader())
			{
				if (!reader.HasRows)
				{
					//returns an empty list if no languages exist in table
					return languages;
				}

				while (reader.Read())
				{
					Language language = new Language();

					language.Id = reader.GetInt32(0);
					language.Code = reader.GetString(1);
					language.Name = reader.GetString(2);

					languages.Add(language);
				}
			}

			return languages;
		}

		public Language GetById(int langId)
		{
			throw new NotImplementedException();
		}

		public void Insert(Language language)
		{
			throw new NotImplementedException();
		}

		public void Update(Language language)
		{
			throw new NotImplementedException();
		}
	}
}
