using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly LocDbContext _dbContext;

        public UserRepository(LocDbContext dbContext)
		{
            _dbContext = dbContext;
        }
		public void Delete(string userId)
		{
			var user = _dbContext.Users.Find(userId);
			if (user != null)
			{
				_dbContext.Users.Remove(user);
				_dbContext.SaveChanges();
			}
		}

		public IEnumerable<User> GetAll()
		{
			return _dbContext.Users;
		}

		public User GetById(string userId)
		{
			return _dbContext.Users.Find(userId);
		}

		public void Insert(User user)
		{
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();
		}

		public void Update(User user)
		{
			_dbContext.Users.Update(user);
			_dbContext.SaveChanges();
		}

		public User GetByEmail(string email)
		{
			return _dbContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
		}

		public IEnumerable<User> GetAllManagers()
		{
			return _dbContext.Users;
		}

		public User GetManagerById(string userId)
		{
			return _dbContext.Users.Find(userId);
		}
	}
}
