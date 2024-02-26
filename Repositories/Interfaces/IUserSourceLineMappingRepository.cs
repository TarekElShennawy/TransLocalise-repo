using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface IUserSourceLineMappingRepository
	{
		void AssignSourceLinesToUser(string userId, IEnumerable<int> sourceLineIds);
		IQueryable<UserSourceLineMapping> GetUserSourceLineMappings(string userId);
	}
}
