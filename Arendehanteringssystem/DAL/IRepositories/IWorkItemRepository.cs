using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IWorkItemRepository
    {
        List<WorkItem> GetAll();
        WorkItem Find(int id);
        WorkItem Add(WorkItem workItem);
        bool Update(WorkItem workItem);
        bool Remove(int id);
        bool SetStatus(int id, bool status);
        bool SetReviewed(int id);
        bool AssignUser(int id, int userId);
        List<WorkItem> GetAllDoneBetweenDates(DateTime startDate, DateTime endDate);
        List<WorkItem> Search(string search);
    }
}
