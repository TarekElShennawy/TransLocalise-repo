using MySqlConnector;
using System.Transactions;
using Translator_Project_Management.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class FileRepository : IFileRepository
	{
		private readonly MySqlDatabase _db;

		private string c_CreateFileCmd = @"INSERT INTO files (project_id, name, type, status) VALUES (@project_id, @name, @type, @status)";
		private string c_GetByProjectId = @"SELECT * FROM files WHERE project_id = @projectId";

		private string c_DeleteFileCmd = @"DELETE FROM files WHERE id = @file_id";

		public FileRepository(MySqlDatabase db)
        {
			_db = db;
		}

        public void Delete(int fileId)
		{
			MySqlCommand cmd = this._db.Connection.CreateCommand();

			cmd.CommandText = c_DeleteFileCmd;
			cmd.Parameters.AddWithValue("@file_id", fileId);

			cmd.ExecuteNonQuery();
		}

		public IEnumerable<Models.Database.File> GetAll()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Models.Database.File> GetByProjectId(int projectId)
		{
			List<Models.Database.File> files = new List<Models.Database.File>();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetByProjectId;

			cmd.Parameters.AddWithValue("@projectId", projectId);

			using(MySqlDataReader reader = cmd.ExecuteReader())
			{
				if(!reader.HasRows)
				{
					//No files are associated with project
					return files;
				}

				while(reader.Read())
				{
					Models.Database.File file = new Models.Database.File()
					{
						Id = reader.GetInt32(0),
						ProjectId = reader.GetInt32(1),
						Name = reader.GetString(2),
						Type = reader.GetString(3),
						Status = reader.GetString(4)
					};

					files.Add(file);
				}
			}

			return files;
		}

		public int Insert(Models.Database.File file, MySqlTransaction transaction)
		{
			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_CreateFileCmd;
			cmd.Transaction = transaction;

			cmd.Parameters.AddWithValue("@name", file.Name);
			cmd.Parameters.AddWithValue("@type", file.Type);
			cmd.Parameters.AddWithValue("@project_id", file.ProjectId);
			cmd.Parameters.AddWithValue("@status", file.Status);

			//Executing the insert query
			cmd.ExecuteNonQuery();

			//Obtain generated Id for the file
			int fileId = (int)cmd.LastInsertedId;

			return fileId;
		}

		public void Update(Models.Database.File file)
		{
			throw new NotImplementedException();
		}
	}
}
