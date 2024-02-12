using MySqlConnector;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class UserRepository : IUserRepository
	{
		private static string c_CreateUserCmd = @"INSERT INTO users (first_name, last_name, email, role, language, skills_and_experience) VALUES 
											(@first_name, @last_name, @email, @role, @language, @skills_and_experience)";

		private static string c_GetUserByIdCmd = @"SELECT * FROM users WHERE id = @id";

		private static string c_GetManagerForProject = @"SELECT * FROM users WHERE id = @id AND role = 'manager'";

		private static string c_GetAllUsersCmd = "SELECT * FROM users";

		private static string c_GetAllManagers = $"{c_GetAllUsersCmd} WHERE role = 'manager'";

		private static string c_DeleteUserCmd = @"DELETE FROM users WHERE id = @id";

		private readonly MySqlDatabase _db;

		public UserRepository(MySqlDatabase db)
		{ 
			_db = db;
		}
		public void Delete(int userId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetAll()
		{
			throw new NotImplementedException();
		}

		public User GetById(int userId)
		{
			throw new NotImplementedException();
		}

		public void Insert(User user)
		{
			throw new NotImplementedException();
		}

		public void Update(User user)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetAllManagers()
		{
			List<User> managers = new List<User>();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetAllManagers;

			using(MySqlDataReader reader = cmd.ExecuteReader())
			{
				if(!reader.HasRows)
				{
					//returns an empty list if no managers exist in table
					return managers;
				}

				while(reader.Read())
				{
					User manager = new User();

					manager.Id = reader.GetInt32(0);
					manager.FirstName = reader.GetString(1);
					manager.LastName = reader.GetString(2);
					manager.Email = reader.GetString(3);
					manager.Role = reader.GetString(4);
					manager.LanguageId = reader.GetInt32(5);
					manager.SkillsAndExperience = reader.GetString(6);

					managers.Add(manager);
				}
			}

			return managers;
		}

		public User GetManagerForProject(int userId)
		{
			User projectManager = new User();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetManagerForProject;

			cmd.Parameters.AddWithValue("@id", userId);

			using (MySqlDataReader reader = cmd.ExecuteReader())
			{
				if (!reader.HasRows)
				{
					//No users are associated with the project
					return null;
				}

				while (reader.Read())
				{
					projectManager.FirstName = reader.GetString(1);
					projectManager.LastName = reader.GetString(2);
					projectManager.Email = reader.GetString(3);
					projectManager.Role = reader.GetString(4);
					projectManager.LanguageId = reader.GetInt32(5);
					projectManager.SkillsAndExperience = reader.GetString(6);
				}
			}

			return projectManager;
		}
	}
}
