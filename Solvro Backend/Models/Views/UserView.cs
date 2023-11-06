using Solvro_Backend.Enums;
using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Models.Views
{
    public class UserView
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        /// <example>1</example>
        public long Id { get; set; }

        /// <summary>
        /// Specialization of the user
        /// </summary>
        /// <example>1</example>
        public Specialization Specialization { get; set; }

        public UserView(User user)
        {
            Id = user.Id;
            Specialization = user.Specialization;
        }
    }
}
