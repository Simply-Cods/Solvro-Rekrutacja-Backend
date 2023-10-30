namespace Solvro_Backend.Models
{
    public class AssignmentView
    {
        public Guid Id { get; set; }
        public List<AssignmentListMember> Propositions { get; set; }

        public AssignmentView(List<(long taskId, long userId)> assignment)
        {
            Id = Guid.NewGuid();
            Propositions = new();
            foreach ((long taskId, long userId) in assignment)
            {
                Propositions.Add(new AssignmentListMember() { TaskId = taskId, UserId = userId });
            }
        }
    }

    public class AssignmentListMember
    {
        public long TaskId { get; set; }
        public long UserId { get; set; }
    }
}
