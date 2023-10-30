using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Repositories
{
    public interface IProjectRepository
    {
        List<Project> GetAllProjects();
        Task<Project> CreateProject(Project project);
        Project? GetProject(long id);
        List<Project> GetProjectsForUser(long userId);
        List<(long taskId, long userId)>? GetAssignment(long projectId);
    }
}
