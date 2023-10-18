using Solvro_Backend.Enums;
using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Models.Views
{
    public class UserView
    {
        public long Id { get; set; }
        public Specialization Specialization { get; set; }

        public UserView(User user)
        {
            Id = user.Id;
            Specialization = user.Specialization;
        }
    }
}
