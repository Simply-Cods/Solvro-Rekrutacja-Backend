using Solvro_Backend.Models.Database;
using STask = Solvro_Backend.Models.Database.Task;

namespace Solvro_Backend.Repositories
{
    public class TaskRepository : BaseRepository<TaskRepository>, ITaskRepository
    {
        public TaskRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<STask> CreateTask(STask task)
        {
            var entry = Database.CreateTask(task);
            await Database.SaveChangesAsync();
            return entry.Entity;
        }

        public (STask? task, bool projectExists) SelectTask(long projectId, long taskId)
        {
            Project? project = Database.SelectProject(projectId);
            if (project == null) return (null, false);

            STask? task = Database.SelectTask(taskId);

            if(task == null || task.Project.Id != projectId)
                return (null, true);

            return (task, true);
        }

        public async Task<STask> UpdateTask(STask task)
        {
            var entry = Database.UpdateTask(task);
            await Database.SaveChangesAsync();
            return entry.Entity;
        }
    }
}
