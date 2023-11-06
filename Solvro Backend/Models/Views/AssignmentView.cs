namespace Solvro_Backend.Models
{
    public class AssignmentView
    {
        /// <summary>
        /// Global unique identifier of the assignment
        /// </summary>
        /// <example>ghso2-sndgy3-sandjsudlcc3414-1msby</example>
        public Guid Id { get; set; }

        /// <summary>
        /// List of proposed pairings
        /// </summary>
        public List<AssignmentListMember> Propositions { get; set; }

        public AssignmentView(List<Assignment> assignments)
        {
            Id = Guid.NewGuid();
            Propositions = new();
            foreach (var assignment in assignments)
            {
                Propositions.Add(new AssignmentListMember() { TaskId = assignment.TaskId, UserId = assignment.UserId });
            }
        }
    }

    public class AssignmentListMember
    {
        /// <summary>
        /// Id of the task
        /// </summary>
        /// <example>1</example>
        public long TaskId { get; set; }

        /// <summary>
        /// Id of the user
        /// </summary>
        /// <example>1</example>
        public long UserId { get; set; }
    }
}
