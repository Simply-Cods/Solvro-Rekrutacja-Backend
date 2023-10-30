using Solvro_Backend.Enums;
using Solvro_Backend.Models.Views;

namespace Solvro_Backend.Logic
{
    public class AssignmentAlgorhythm
    {

        public static List<(long taskId, long userId)> AssignSimple(ProjectFullView project)
        {
            var unassignedTasks = project.Tasks.Where(t => t.AssignedUser == null && t.State != TaskState.Done);

            Dictionary<Specialization, List<TaskView>> tasksBySpecialization = new();
            List<(long taskId, long userId)> pairings = new();

            foreach (var task in unassignedTasks)
            {
                if(tasksBySpecialization.TryGetValue(task.Specialization, out var list))
                {
                    list.Add(task);
                }
                else
                {
                    tasksBySpecialization.Add(task.Specialization, new List<TaskView>() { task });
                }
            }

            foreach(var list in tasksBySpecialization.Values)
            {
                list.Sort((x, y) => x.Estimation.CompareTo(y.Estimation));
            }
            
            foreach(var key in tasksBySpecialization.Keys)
            {
                var tasks = tasksBySpecialization[key];

                Dictionary<int, List<DeveloperStats>> devStatsByEstimation = new();

                foreach (var dev in project.Members.Where(m => m.Specialization == key).Select(u => new DeveloperStats(project, u)))
                {
                    int estimation = dev.HighestEstimation == null ? 0 : dev.HighestEstimation.Value;
                    if (devStatsByEstimation.TryGetValue(estimation, out var list))
                    {
                        list.Add(dev);
                    }
                    else
                    {
                        devStatsByEstimation.Add(estimation, new List<DeveloperStats>() { dev });
                    }
                }
                if (devStatsByEstimation.Count == 0)
                    continue;

                foreach(var list in devStatsByEstimation.Values)
                {
                    list.Sort((x, y) => x.CompareTo(y));
                }

                foreach (var task in tasks)
                {
                    var bestFit = GetBestFit(devStatsByEstimation, task);
                    if (bestFit == null)
                        continue;
                    pairings.Add((task.Id, bestFit.developer.Id));
                }   
            }
            return pairings;
        }

        private static DeveloperStats? GetBestFit(Dictionary<int, List<DeveloperStats>> devsByEstimation, TaskView task)
        {
            int upperLimit = task.Estimation;
            int lowerLimit = task.Estimation;

            DeveloperStats? bestFit = null;

            while(bestFit == null)
            {
                if(devsByEstimation.TryGetValue(lowerLimit, out var list))
                {
                    bestFit = list[0];
                    list[0].assignedTasks++;
                }
                else if(devsByEstimation.TryGetValue(upperLimit, out list))
                {
                    bestFit = list[0];
                    list[0].assignedTasks++;
                }
                else 
                {
                    upperLimit++;
                    lowerLimit++;
                }
                list?.Sort((x, y) => x.CompareTo(y));

                if (devsByEstimation.Keys.Last() < upperLimit && devsByEstimation.Keys.First() > lowerLimit)
                    break;
            }

            return bestFit;
        }
    }

    public class DeveloperStats
    {
        public UserView developer;
        public List<TaskView> completedTasks;
        public int? HighestEstimation => completedTasks.FirstOrDefault()?.Estimation;
        public int assignedTasks;

        public DeveloperStats(ProjectFullView project, UserView developer)
        {
            this.developer = developer;

            completedTasks = project.Tasks.Where(t => t.AssignedUser?.Id == developer.Id && t.State == TaskState.Done).ToList();
            completedTasks.Sort((x, y) =>
            {
                int order = x.Estimation.CompareTo(y.Estimation);
                if (order == 0)
                {
                    TimeSpan xTime = new(x.CompletedAt!.Value.Ticks - x.CreatedAt.Ticks);
                    TimeSpan yTime = new(y.CompletedAt!.Value.Ticks - y.CreatedAt.Ticks);
                    return xTime.CompareTo(yTime);
                }
                return order;
            });

            assignedTasks = project.Tasks.Where(t => t.AssignedUser?.Id == developer.Id && t.State != TaskState.Done).Count();
        }

        /// <summary>
        /// Compare based on (in this order): highest estimation of completed tasks, number of currently assigned tasks, 
        /// timespan between creation and completion of the highest rated task
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(DeveloperStats other)
        {
            if (!HighestEstimation.HasValue && !other.HighestEstimation.HasValue)
                return 0;
            if (!HighestEstimation.HasValue || !other.HighestEstimation.HasValue)
                return HighestEstimation.HasValue ? 1 : -1;
            int order = HighestEstimation.Value.CompareTo(other.HighestEstimation.Value);

            if (order == 0)
            {
                order = assignedTasks.CompareTo(other.assignedTasks);
            }

            if (order == 0)
            {
                TimeSpan thisTime = new(completedTasks[0].CompletedAt!.Value.Ticks - completedTasks[0].CreatedAt.Ticks);
                TimeSpan otherTime = new(other.completedTasks[0].CompletedAt!.Value.Ticks - other.completedTasks[0].CreatedAt.Ticks);
                order = thisTime.CompareTo(otherTime);
            }
            return order;
        }
    }
}
