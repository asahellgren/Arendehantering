using System.Collections.Generic;

namespace DAL.Entities
{
    public sealed class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
