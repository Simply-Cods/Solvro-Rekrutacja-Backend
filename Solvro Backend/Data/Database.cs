using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Solvro_Backend.Models.Database;

using STask = Solvro_Backend.Models.Database.Task;
using Task = System.Threading.Tasks.Task;

namespace Solvro_Backend.Data
{
    public class Database
    {
        private readonly DataContext _context;

        public Database(DataContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Project Handling

        public List<Project> SelectAllProjects()
        {
            return _context.Projects.AsQueryable().ToList();
        }

        public EntityEntry<Project> CreateProject(Project project)
        {
            return _context.Projects.Add(project);
        }

        public Project? SelectProject(long id)
        {
            return _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectMemberMappings).ThenInclude(m => m.User) //if this was enterprise, this would be unacceptable, but it works
                .Include(p => p.Tasks)
                .AsQueryable().FirstOrDefault(p => p.Id == id);
        }

        #endregion

        #region User Handling

        public List<User> SelectAllUsers()
        {
            return _context.Users.AsQueryable().ToList();
        }

        public EntityEntry<User> CreateUser(User user)
        {
            return _context.Users.Add(user);
        }

        public User? SelectUser(long id)
        {
            return _context.Users.AsQueryable().FirstOrDefault(u => u.Id == id);
        }

        #endregion

        #region ProjectMemberMapping Handling

        public EntityEntry<ProjectMemberMapping> CreateProjectMemberMapping(ProjectMemberMapping mapping)
        {
            return _context.ProjectMemberMappings.Add(mapping);
        }

        public ProjectMemberMapping? SelectProjectMemberMapping(long id)
        {
            return _context.ProjectMemberMappings.AsQueryable().FirstOrDefault(m => m.Id == id);
        }

        #endregion
    }
}
