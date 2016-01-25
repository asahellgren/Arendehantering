using System.Collections.Generic;

namespace DAL.Entities
{
    public sealed class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> TeamUsers { get; set; }
    }
}
