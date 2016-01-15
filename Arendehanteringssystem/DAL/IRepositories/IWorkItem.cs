using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IWorkItem
    {
        List<WorkItem> GetAll();
        WorkItem Find(int id);
        WorkItem Add(WorkItem workItem);
        WorkItem Update(WorkItem workItem);
        void Remove(int id);
    }
}
