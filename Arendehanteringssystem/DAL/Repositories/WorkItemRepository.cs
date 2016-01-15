using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class WorkItemRepository : IWorkItem
    {
        public WorkItem Add(WorkItem workItem)
        {
            throw new NotImplementedException();
        }

        public WorkItem Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<WorkItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public WorkItem Update(WorkItem workItem)
        {
            throw new NotImplementedException();
        }
    }
}
