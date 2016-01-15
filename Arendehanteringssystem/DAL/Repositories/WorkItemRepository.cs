using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class WorkItemRepository : IWorkItem
    {
        private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["Arandehantering"].ConnectionString);
        public WorkItem Add(WorkItem workItem)
        {
            var sqlQuery = "INSERT INTO WorkItem (Title, Description, DateCreated, DateDone, Reviewed, UserId) VALUES(@Title, @Description, @DateCreated, @DateDone, @Reviewed, @UserId)";
            var workItemId = _db.Query<int>(sqlQuery, workItem).Single();
            workItem.Id = workItemId;
            return workItem;
        }

        public WorkItem Find(int id)
        {
            return _db.Query<WorkItem>("SELECT * FROM WorkItem WHERE Id = @id").SingleOrDefault();
        }

        public List<WorkItem> GetAll()
        {
            return _db.Query<WorkItem>("SELECT * FROM WorkItem").ToList();
        }

        public void Remove(int id)
        {
             _db.Query("DELETE From WorkItem WHERE Id = @id");
        }

        public WorkItem Update(WorkItem workItem)
        {
            var sqlQuery =
                "UPDATE WorkItem SET Title = @Title, Description = @Description, DateCreated = @DateCreated, DateDone = @DateDone, Reviewed = @Reviewed WHERE Id = @id";
            _db.Execute(sqlQuery, workItem);
            return workItem;
        }
    }
}
