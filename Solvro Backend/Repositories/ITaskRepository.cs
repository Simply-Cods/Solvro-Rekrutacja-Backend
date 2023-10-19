using STask = Solvro_Backend.Models.Database.Task;

namespace Solvro_Backend.Repositories
{
    public interface ITaskRepository
    {
        public Task<STask> CreateTask(STask task);
        public Task<STask> UpdateTask(STask task);
        public (STask? task, bool projectExists) SelectTask(long projectId, long taskId);
    }
}
