using Solvro_Backend.Data;
using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Repositories
{
    public class ProjectRepository : BaseRepository<ProjectRepository>, IProjectRepository
    {
        public ProjectRepository(IServiceProvider provider): base(provider) { }

        public List<Project> GetAllProjects()
        {
            return Database.SelectAllProjects();
        }

        public async Task<Project> CreateProject(Project project)
        {
            var entry = Database.CreateProject(project);
            await Database.SaveChangesAsync();
            return entry.Entity;
        }

        public Project? GetProject(long id)
        {
            return Database.SelectProject(id);
        }
    }
}
