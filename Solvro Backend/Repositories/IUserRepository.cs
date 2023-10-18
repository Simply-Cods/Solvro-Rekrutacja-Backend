using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        Task<User> CreateUser(User user);
        User? GetUser(long id);
    }
}
