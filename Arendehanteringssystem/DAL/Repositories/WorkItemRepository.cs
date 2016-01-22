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
        private readonly IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["Arandehantering"].ConnectionString);
        public WorkItem Add(WorkItem workItem)
        {
            var sqlQuery = "INSERT INTO WorkItem (Title, Description, DateCreated, DateDone, Reviewed, CreatedByUserId, UserId, TeamId) " +
                           "VALUES(@Title, @Description, @DateCreated, @DateDone, @Reviewed, @CreatedByUserId, @UserId, @TeamId)" +
                           "SELECT SCOPE_IDENTITY()";

            var workItemId = _db.Query<int>(sqlQuery, workItem).Single();
            workItem.Id = workItemId;
            return workItem;
        }

        public WorkItem Find(int id)
        {
            var workItem = _db.Query<WorkItem>("SELECT * FROM WorkItem WHERE Id = @id").SingleOrDefault();
            var issues = _db.Query<List<Issue>>("SELECT * FROM Issue WHERE Id = @Id", new { workItem.Id }).ToList();
            workItem.Issues = issues;
            return workItem;
        }

        public List<WorkItem> GetAll()
        {
            return _db.Query<WorkItem>("SELECT * FROM WorkItem").ToList();
        }

        public bool Remove(int id)
        {
            var sqlQuery = "DELETE From WorkItem WHERE Id = @id";
            var affectedRows = _db.Execute(sqlQuery, id);
            return affectedRows == 1;
        }

        public bool SetDone(int id)
        {
            var dateDone = DateTime.UtcNow;
            var sqlQuery = "UPDATE WorkItem SET DateDone = @DateDone WHERE Id = @Id";
            var affectedRows = _db.Execute(sqlQuery,new { id, dateDone});
            return affectedRows == 1;
        }

        public bool SetReviewed(int id)
        {
            var sqlQuery = "UPDATE WorkItem SET Reviewed = true WHERE Id = @Id";
            var affectedRows = _db.Execute(sqlQuery, id);
            return affectedRows == 1;
        }

        public bool AssignUser(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public List<WorkItem> FindString(string stringToFind)
        {
            throw new NotImplementedException();
        }

        public List<WorkItem> GetAllDoneBetweenDates(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public WorkItem Update(WorkItem workItem)
        {
            var sqlQuery = "UPDATE WorkItem SET Title = @Title, Description = @Description, DateCreated = @DateCreated, DateDone = @DateDone, Reviewed = @Reviewed, UserId = @UserId, TeamId = @TeamId WHERE Id = @id";
            _db.Execute(sqlQuery, workItem);
            return workItem;
        }


    }
}
