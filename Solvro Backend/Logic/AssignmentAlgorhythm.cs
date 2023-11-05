using Solvro_Backend.Enums;
using Solvro_Backend.Models.Views;
using Solvro_Backend.Models;

namespace Solvro_Backend.Logic
{
    public class AssignmentAlgorhythm
    {

        ///
        /// 1. get list of all tasks and developers
        /// 2. order devs in a list based on number of tasks assigned (ascending)
        /// 2.5 order tasks to assign based on their estimaton (ascending)
        /// 3. get first dev in the list and find him a task with the closest estimation to his historical highest
        /// 4. reorder list and goto step 3
        ///
        public static List<Assignment> AssignSimple(ProjectFullView project)
        {
            Dictionary<Specialization, List<TaskView>> taskDict = new();
            Dictionary<Specialization, List<DeveloperStats>> developerDict = new();
            List<Assignment> pairings = new();

            foreach(var dev in project.Members)
            {
                if (!developerDict.ContainsKey(dev.Specialization))
                    developerDict.Add(dev.Specialization, new List<DeveloperStats>());

                developerDict[dev.Specialization].Add(new DeveloperStats(dev, project));
            }

            foreach(var list in developerDict.Values)
            {
                list.Sort((a, b) => a.CompareTo(b));
            }

            foreach(var task in project.Tasks)
            {
                if (task.State != TaskState.Open)
                    continue;
                if (!taskDict.ContainsKey(task.Specialization))
                    taskDict.Add(task.Specialization, new List<TaskView>());

                taskDict[task.Specialization].Add(task);
            }

            foreach(var list in taskDict.Values)
            {
                list.Sort((a, b) => a.Estimation.CompareTo(b.Estimation));
            }

            foreach(var tasks in taskDict.Values)
            {
                if (!developerDict.ContainsKey(tasks[0].Specialization))
                    continue;

                var devs = developerDict[tasks[0].Specialization];

                while(tasks.Count > 0)
                {
                    var dev = devs[0];
                    var task = tasks.MinBy(t => Math.Abs(t.Estimation - dev.highestHistoricalEstimation));
                    pairings.Add(new Assignment() { TaskId = task!.Id, UserId = dev.user.Id});
                    dev.assignedTaskCost += task.Estimation;
                    devs.Sort((a, b) => a.CompareTo(b));
                    tasks.Remove(task);
                }
            }

            return pairings;
        }

        public class DeveloperStats
        {
            public UserView user;
            public int assignedTaskCost;
            public int highestHistoricalEstimation;
            public TimeSpan quickestHistoricalCompletion;

            public DeveloperStats(UserView dev, ProjectFullView project)
            {
                user = dev;
                highestHistoricalEstimation = 0;
                quickestHistoricalCompletion = TimeSpan.Zero;
                assignedTaskCost = 0;

                foreach(var t in project.Tasks)
                {
                    if (t.AssignedUser?.Id != dev.Id)
                        continue;

                    if(t.State == TaskState.Done)
                    {
                        highestHistoricalEstimation = t.Estimation > highestHistoricalEstimation ? t.Estimation : highestHistoricalEstimation;
                        var span = TimeSpan.FromTicks(t.CompletedAt!.Value.Ticks - t.CreatedAt.Ticks);
                        quickestHistoricalCompletion = span < quickestHistoricalCompletion ? span : quickestHistoricalCompletion;
                    }
                    else
                    {   
                        assignedTaskCost += t.Estimation;
                    }
                }
            }

            public int CompareTo(DeveloperStats other)
            {
                if(assignedTaskCost != other.assignedTaskCost)
                    return assignedTaskCost.CompareTo(other.assignedTaskCost);

                return quickestHistoricalCompletion.CompareTo(other.quickestHistoricalCompletion);
            }
        }
    }
}
