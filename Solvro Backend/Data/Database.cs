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
                .Include(p => p.ProjectMemberMappings).ThenInclude(m => m.User)
                .Include(p => p.Tasks).ThenInclude(t => t.AssignedUser)
                .Include(p => p.Tasks).ThenInclude(t => t.Creator)
                .AsQueryable().FirstOrDefault(p => p.Id == id);
        }

        public List<Project> SelectProjectsForUser(long userId)
        {
            return _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectMemberMappings).ThenInclude(m => m.User)
                .AsQueryable().Where(p => p.Owner.Id == userId || p.ProjectMemberMappings.Any(m => m.User.Id == userId))
                .Include(p => p.Tasks).ThenInclude(t => t.AssignedUser)
                .Include(p => p.Tasks).ThenInclude(t => t.Creator)
                .ToList();
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

        #region Task Handling

        public EntityEntry<STask> CreateTask(STask task)
        {
            return _context.Tasks.Add(task);
        }

        public STask? SelectTask(long id)
        {
            return _context.Tasks.AsQueryable()
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Include(t => t.Creator)
                .FirstOrDefault(t => t.Id == id);
        }

        public EntityEntry<STask> UpdateTask(STask task)
        {
            return _context.Tasks.Update(task);
        }

        #endregion
    }
}
