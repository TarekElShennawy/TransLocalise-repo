using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private readonly LocDbContext _dbContext;

        public ClientRepository(LocDbContext dbContext)
        {
			_dbContext = dbContext;
        }

        public void Delete(int clientId)
		{
			Client client = _dbContext.Clients.Find(clientId);

			if(client != null)
			{
				_dbContext.Clients.Remove(client);
				_dbContext.SaveChanges();
			}
		}

		public IEnumerable<Client> GetAll()
		{
			return _dbContext.Clients;
		}

		public Client GetById(int clientId)
		{
            return _dbContext.Clients.Find(clientId);
        }

		public void Insert(Client client)
		{
			_dbContext.Add(client);
			_dbContext.SaveChanges();
		}

		public void Update(Client client)
		{
			_dbContext.Update(client);
			_dbContext.SaveChanges();
		}
	}
}
