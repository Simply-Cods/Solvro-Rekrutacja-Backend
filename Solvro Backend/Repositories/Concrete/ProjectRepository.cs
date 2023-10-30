using Solvro_Backend.Data;
using Solvro_Backend.Logic;
using Solvro_Backend.Models.Database;
using Solvro_Backend.Models.Views;

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

        public List<Project> GetProjectsForUser(long userId)
        {
            return Database.SelectProjectsForUser(userId);
        }

        public List<(long taskId, long userId)>? GetAssignment(long projectId)
        {
            var project = GetProject(projectId);
            if (project == null)
                return null;

            return AssignmentAlgorhythm.AssignSimple(new ProjectFullView(project));
        }
    }
}
