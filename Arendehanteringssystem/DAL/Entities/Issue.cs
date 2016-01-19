using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public sealed class Issue
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? DateDone { get; set; }
        public DateTime? DateCreated { get; set; }
        public int WorkItemId { get; set; }
    }
}
