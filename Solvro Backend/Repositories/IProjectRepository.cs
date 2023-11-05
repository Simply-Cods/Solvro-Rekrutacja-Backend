using Solvro_Backend.Models.Database;
using Solvro_Backend.Models;

namespace Solvro_Backend.Repositories
{
    public interface IProjectRepository
    {
        List<Project> GetAllProjects();
        Task<Project> CreateProject(Project project);
        Project? GetProject(long id);
        List<Project> GetProjectsForUser(long userId);
        List<Assignment>? GetAssignment(long projectId);
        Task<bool> ApplyAssignment(long projectId, List<Assignment> assignments);
    }
}
