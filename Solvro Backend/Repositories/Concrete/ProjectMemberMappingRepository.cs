using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Repositories
{
    public class ProjectMemberMappingRepository : BaseRepository<ProjectMemberMappingRepository>, IProjectMemberMappingRepository
    {
        public ProjectMemberMappingRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async System.Threading.Tasks.Task BulkCreateMapping(ICollection<ProjectMemberMapping> mappings)
        {
            if (mappings.Count == 0) return;
            foreach (var mapping in mappings)
            {
                Database.CreateProjectMemberMapping(mapping);
            }
            await Database.SaveChangesAsync();
        }

        public async Task<ProjectMemberMapping> CreateMapping(ProjectMemberMapping mapping)
        {
            var entry = Database.CreateProjectMemberMapping(mapping);
            await Database.SaveChangesAsync();
            return entry.Entity;
        }

        public ProjectMemberMapping? GetMapping(long id)
        {
            return Database.SelectProjectMemberMapping(id);
        }
    }
}
