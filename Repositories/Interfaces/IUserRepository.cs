using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //Gets all users
        IEnumerable<User> GetAll();

        //Gets all managers
        IEnumerable<User> GetAllManagers();

        //Gets user by ID
        User GetById(string userId);

        //Gets user by Email (used for authentication and authorisation)
        User GetByEmail(string email);

        //Inserts a new user record
        void Insert(User user);

        //Updates a user record
        void Update(User user);

        //Deletes a user record
        void Delete(string userId);

        //Gets manager record by their ID
        User GetManagerById(string userId);
    }
}
