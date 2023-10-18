using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Repositories
{
    public interface IProjectMemberMappingRepository
    {
        ProjectMemberMapping? GetMapping(long id);
        Task<ProjectMemberMapping> CreateMapping(ProjectMemberMapping mapping);
        System.Threading.Tasks.Task BulkCreateMapping(ICollection<ProjectMemberMapping> mappings);
    }
}
