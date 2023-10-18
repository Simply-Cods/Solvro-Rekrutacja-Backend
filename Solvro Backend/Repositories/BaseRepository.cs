using Solvro_Backend.Data;

namespace Solvro_Backend.Repositories
{
    public abstract class BaseRepository<T>
    {
        protected ILogger<T> Logger;
        protected Database Database;

        public BaseRepository(IServiceProvider serviceProvider) 
        {
            Logger = serviceProvider.GetRequiredService<ILogger<T>>();
            Database = serviceProvider.GetRequiredService<Database>();
        }
    }
}
