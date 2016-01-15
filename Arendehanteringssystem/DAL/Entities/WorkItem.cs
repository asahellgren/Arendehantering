using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class WorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDone { get; set; }
        public bool Reviewed { get; set; }
        public int PriorityIndex { get; set; }
        public int? UserId { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
