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
    public class WorkItemRepository : IWorkItemRepository
    {
        private readonly IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);
        public WorkItem Add(WorkItem workItem)
        {
            var sqlQuery = "INSERT INTO WorkItem (Title, Description, DateCreated, DateDone, Reviewed, CreatedByUserId, UserId, TeamId) " +
                           "VALUES(@Title, @Description, @DateCreated, @DateDone, @Reviewed, @CreatedByUserId, @UserId, @TeamId)" +
                           "SELECT SCOPE_IDENTITY()";

            var workItemId = _con.Query<int>(sqlQuery, workItem).Single();
            workItem.Id = workItemId;
            return workItem;
        }

        public WorkItem Find(int id)
        {
            var workItem = _con.Query<WorkItem>("SELECT * FROM WorkItem WHERE Id = @id").SingleOrDefault();
            var issues = _con.Query<List<Issue>>("SELECT * FROM Issue WHERE Id = @Id", new { workItem.Id }).ToList();
            workItem.Issues = issues;
            return workItem;
        }

        public List<WorkItem> GetAll()
        {
            return _con.Query<WorkItem>("SELECT * FROM WorkItem").ToList();
        }

        public bool Remove(int id)
        {
            var sqlQuery = "DELETE From WorkItem WHERE Id = @id";
            var affectedRows = _con.Execute(sqlQuery, id);
            return affectedRows == 1;
        }

        public WorkItem Update(WorkItem workItem)
        {
            var sqlQuery = "UPDATE WorkItem SET Title = @Title, Description = @Description, DateCreated = @DateCreated, DateDone = @DateDone, Reviewed = @Reviewed, UserId = @UserId, TeamId = @TeamId WHERE Id = @id";
            _con.Execute(sqlQuery, workItem);
            return workItem;
        }

        public bool SetStatus(int id, bool status)
        {
            string sqlQuery = "";
            var dateDone = DateTime.UtcNow;
            if (status)
            {
                sqlQuery = "UPDATE WorkItem SET DateDone = @DateDone WHERE Id = @Id";
            }
            else
            {
                sqlQuery = "UPDATE WorkItem SET DateDone = null WHERE Id = @Id";
            }
            var affectedRows = _con.Execute(sqlQuery, new { id, dateDone });
            return affectedRows == 1;
        }

        public bool SetReviewed(int id)
        {
            var sqlQuery = "UPDATE WorkItem SET Reviewed = true WHERE Id = @Id";
            var affectedRows = _con.Execute(sqlQuery, id);
            return affectedRows == 1;
        }

        public bool AssignUser(int id, int userId)
        {
            var sqlQuery = "UPDATE WorkItem SET UserId = @UserId WHERE Id = @Id";
            var affectedRows = _con.Execute(sqlQuery, new { id, userId });
            return affectedRows == 1;
        }

        public List<WorkItem> FindString(string stringToFind)
        {
            var sqlQuery = "SELECT * FROM WorkItem WHERE CHARINDEX(@StringToFind, Description) > 0";
            List<WorkItem> result = _con.Query<WorkItem>(sqlQuery, stringToFind).ToList();
            return result;
        }

        public List<WorkItem> GetAllDoneBetweenDates(DateTime startDate, DateTime endDate)
        {
            var sqlQuery = "SELECT * FROM WorkItem WHERE DateDone < @endDate AND DateDone > @startDate";
            List<WorkItem> result = _con.Query<WorkItem>(sqlQuery, new { endDate, startDate }).ToList();
            return result;
        }

        public List<WorkItem> Search(string search)
        {
            try
            {
                var sqlQuery =
               "SELECT * FROM [WorkItem] WHERE Description LIKE @search";
                search = "%" + search + "%";
                var workItems = _con.Query<WorkItem>(sqlQuery, new { search }).ToList();
                return workItems;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
