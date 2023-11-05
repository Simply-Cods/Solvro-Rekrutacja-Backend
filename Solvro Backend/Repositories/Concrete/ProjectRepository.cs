using Solvro_Backend.Data;
using Solvro_Backend.Logic;
using Solvro_Backend.Models.Database;
using Solvro_Backend.Models.Views;
using Solvro_Backend.Models;

namespace Solvro_Backend.Repositories
{
    public class ProjectRepository : BaseRepository<ProjectRepository>, IProjectRepository
    {
        private ITaskRepository _taskRepository;
        private IUserRepository _userRepository;
        public ProjectRepository(IServiceProvider provider): base(provider) 
        {
            _taskRepository = provider.GetRequiredService<ITaskRepository>();
            _userRepository = provider.GetRequiredService<IUserRepository>();
        }

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

        public List<Assignment>? GetAssignment(long projectId)
        {
            var project = GetProject(projectId);
            if (project == null)
                return null;

            return AssignmentAlgorhythm.AssignSimple(new ProjectFullView(project));
        }

        public async Task<bool> ApplyAssignment(long projectId, List<Assignment> assignments)
        {
            var transaction = await Database.StartTransaction();
            foreach(var assign in assignments)
            {
                var (task, _) = _taskRepository.SelectTask(projectId, assign.TaskId);
                var user = _userRepository.GetUser(assign.UserId);
                if (task == null || user == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                
                task.AssignedUser = user;
                task.State = Enums.TaskState.ToDo;
                await _taskRepository.UpdateTask(task);
            }
            await transaction.CommitAsync();

            return true;
        }
    }
}
