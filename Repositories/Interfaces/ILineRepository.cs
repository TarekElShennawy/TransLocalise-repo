using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface ILineRepository<TLine> where TLine : Line
	{
		//Gets all lines
		IEnumerable<TLine> GetAll();

		//Gets lines for a specific file
		IEnumerable<TLine> GetForFile(int fileId);

		//Inserts a new line record and returns the ID of it
		int Insert(TLine line);

        //Updates a line record
        void Update(TLine line);

		//Deletes a line record
		void Delete(int lineId);
	}
}
