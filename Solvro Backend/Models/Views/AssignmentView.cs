namespace Solvro_Backend.Models
{
    public class AssignmentView
    {
        public Guid Id { get; set; }
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
        public long TaskId { get; set; }
        public long UserId { get; set; }
    }
}
