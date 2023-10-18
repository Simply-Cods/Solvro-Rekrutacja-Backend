using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Repositories
{
    public class UserRepository : BaseRepository<UserRepository>, IUserRepository
    {
        public UserRepository(IServiceProvider provider): base(provider) { }

        public async Task<User> CreateUser(User user)
        {
            var entry = Database.CreateUser(user);
            await Database.SaveChangesAsync();
            return entry.Entity;
        }

        public List<User> GetAllUsers()
        {
            return Database.SelectAllUsers();
        }

        public User? GetUser(long id)
        {
            return Database.SelectUser(id);
        }
    }
}
