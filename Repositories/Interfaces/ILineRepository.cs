using MySqlConnector;
using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface ILineRepository
	{
		//Gets all source lines
		IEnumerable<Line> GetAllSourceLines();

		//Gets all translations
		IEnumerable<Line> GetAllTranslations();

		//Gets line by ID
		Line GetById(int lineId);

		//Inserts a new line record
		void Insert(Line line, MySqlTransaction transaction);

		//Updates a line record
		void Update(Line line);

		//Deletes a line record
		void Delete(int lineId);
	}
}
