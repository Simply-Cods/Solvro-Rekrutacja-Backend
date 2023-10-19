using Solvro_Backend.Models.Database;
using STask = Solvro_Backend.Models.Database.Task;

namespace Solvro_Backend.Logic
{
    public class AssignmentAlgorhythm
    {
        private readonly Project _project;

        public AssignmentAlgorhythm(Project project)
        {
            _project = project;
        }

        public void AssignSimple()
        {
            List<STask> tasks = _project.Tasks.Where(t => t.State != Enums.TaskState.Done).ToList();
            var unassignedTasks = tasks;
            var memberHighScores = new Dictionary<User, int>();



        }
    }
}
