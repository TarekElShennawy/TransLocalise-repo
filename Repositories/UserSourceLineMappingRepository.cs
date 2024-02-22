using System.Data.Entity;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class UserSourceLineMappingRepository : IUserSourceLineMappingRepository
	{
		private readonly LocDbContext _dbContext;

		public UserSourceLineMappingRepository(LocDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void AssignSourceLinesToUser(string userId, IEnumerable<int> sourceLineIds)
		{
			//Get user entity from DbContext's users
			var user = _dbContext.Users.Include(u => u.UserSourceLineMappings).Single(u => u.Id.Equals(userId));

			if(user != null)
			{
				//Create new UserSourceLineMapping entity for each SourceLine Id
				foreach (int sourceLineId in sourceLineIds)
				{
					var sourceTextMapping = new UserSourceLineMapping
					{
						UserId = userId,
						SourceLineId = sourceLineId
					};

					user.UserSourceLineMappings.Add(sourceTextMapping);
				}

				//Save changes to the database
				_dbContext.SaveChanges();
			}
		}

		public IEnumerable<SourceLine> GetUserSourceLines(string userId)
		{
			return _dbContext.Users.Include(u => u.UserSourceLineMappings)
								   .Where(u => u.Id.Equals(userId))
								   .SelectMany(u => u.UserSourceLineMappings)
								   .Select(m => m.SourceLine);
		}
	}
}
