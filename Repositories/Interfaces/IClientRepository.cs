using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Repositories.Interfaces
{
	public interface IClientRepository
	{
		//Gets all clients
		IEnumerable<Client> GetAll();

		//Gets client by ID
		Client GetById(int clientId);

		//Inserts a new client record
		void Insert(Client client);

		//Updates a client record
		void Update(Client client);

		//Deletes a client record
		void Delete(int clientId);

		Client GetClientForProject(int projectId);
	}
}
