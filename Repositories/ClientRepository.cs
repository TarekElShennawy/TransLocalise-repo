using MySqlConnector;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private static string c_GetAllClientsCmd = "SELECT * FROM clients";

		private static string c_GetClientForProjectCmd = @"SELECT * FROM clients WHERE id = @id";

		private readonly MySqlDatabase _db;

        public ClientRepository(MySqlDatabase db)
        {
			_db = db;
        }

        public void Delete(int clientId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Client> GetAll()
		{
			List<Client> clients = new List<Client>();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetAllClientsCmd;

			using (MySqlDataReader reader = cmd.ExecuteReader())
			{
				if (!reader.HasRows)
				{
					//returns an empty list if no projects exist in table
					return null;
				}

				while (reader.Read())
				{
					Client client = new Client();

					client.Id = reader.GetInt32(0);
					client.Name = reader.GetString(1);
					client.ContactName = reader.GetString(2);
					client.Email = reader.GetString(3);
					client.PhoneNumber = reader.GetString(4);
					client.BillingAddress = reader.GetString(5);

					clients.Add(client);
				}
			}

			return clients;
		}

		public Client GetClientForProject(int clientId)
		{
			Client projectClient = new Client();

			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.CommandText = c_GetClientForProjectCmd;

			cmd.Parameters.AddWithValue("@id", clientId);

			using (MySqlDataReader reader = cmd.ExecuteReader())
			{
				if(!reader.HasRows)
				{
					//No clients are associated with the project
					return null;
				}

				while(reader.Read())
				{
					projectClient.Id = reader.GetInt32(0);
					projectClient.Name = reader.GetString(1);
					projectClient.ContactName = reader.GetString(2);
					projectClient.Email = reader.GetString(3);
					projectClient.PhoneNumber = reader.GetString(4);
					projectClient.BillingAddress = reader.GetString(5);
				}
			}

			return projectClient;
		}

		public Client GetById(int clientId)
		{
			throw new NotImplementedException();
		}

		public void Insert(Client client)
		{
			throw new NotImplementedException();
		}

		public void Update(Client client)
		{
			throw new NotImplementedException();
		}
	}
}
