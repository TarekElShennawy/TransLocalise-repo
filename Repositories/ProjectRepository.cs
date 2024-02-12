using MySqlConnector;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private string c_CreateProjectCmd = @"INSERT INTO projects (name, client_id, manager_id, start_date, due_date, status, description) VALUES 
											(@name, @clientId, @managerId, @startDate, @dueDate, @status, @description)";

		private string c_GetProjectByIdCmd = @"SELECT * FROM projects WHERE id = @id";

		private string c_GetAllProjectsCmd = "SELECT * FROM projects";

		private string c_DeleteProjectCmd = @"DELETE FROM projects WHERE id = @id";

		private readonly MySqlDatabase _db;

        public ProjectRepository(MySqlDatabase db)
        {
			_db = db;
        }

		public Project GetById(int projectId)
		{
			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetProjectByIdCmd;

			cmd.Parameters.AddWithValue("@id", projectId);

			using(MySqlDataReader reader = cmd.ExecuteReader())
			{
				if(reader.Read())
				{
					Project project = new Project();

					project.Id = reader.GetInt32(0);
					project.Name = reader.GetString(1);
					project.ClientId = reader.GetInt32(2);
					project.ManagerId = reader.GetInt32(3);
					project.StartDate = reader.GetDateOnly(4);
					project.DueDate = reader.GetDateOnly(5);
					project.Status = reader.GetString(6);
					project.Description = reader.GetString(7);

					return project;
				}
				else
				{
					//No project was found with that Id
					return null;
				}
			}
		}

		public IEnumerable<Project> GetAll()
		{
			List<Project> projects = new List<Project>();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetAllProjectsCmd;

			using(MySqlDataReader reader = cmd.ExecuteReader())
			{
				if(!reader.HasRows)
				{
					//returns an empty list if no projects exist in table
					return projects;
				}

				while(reader.Read())
				{
					Project project = new Project()
					{
						Id = reader.GetInt32(0),
						Name = reader.GetString(1),
						ClientId = reader.GetInt32(2),
						ManagerId = reader.GetInt32(3),
						StartDate = reader.GetDateOnly(4),
						DueDate = reader.GetDateOnly(5),
						Status = reader.GetString(6),
						Description = reader.GetString(7)
					};

					projects.Add(project);
				}				
			}

			return projects;
		}

        public void Insert(Project project)
		{
			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_CreateProjectCmd;
			
			cmd.Parameters.AddWithValue("@name", project.Name);
			cmd.Parameters.AddWithValue("@clientId", project.ClientId);
			cmd.Parameters.AddWithValue("@managerId", project.ManagerId);
			cmd.Parameters.AddWithValue("@startDate", project.StartDate);
			cmd.Parameters.AddWithValue("@dueDate", project.DueDate);
			cmd.Parameters.AddWithValue("@status", project.Status);
			cmd.Parameters.AddWithValue("@description", project.Description);

			cmd.ExecuteNonQuery();
		}

		public void Update(Project project)
		{
			throw new NotImplementedException();
		}

		public void Delete(int projectId)
		{
			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_DeleteProjectCmd;

			cmd.Parameters.AddWithValue("@id", projectId);

			cmd.ExecuteNonQuery();
		}
	}
}
