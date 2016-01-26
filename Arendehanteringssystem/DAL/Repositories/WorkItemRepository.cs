using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public sealed class WorkItemRepository : IWorkItemRepository
    {
        private readonly SqlConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public List<WorkItem> GetAll()
        {
            _con.OpenWithRetry();
            try
            {
                return _con.QueryWithRetry<WorkItem>("SELECT * FROM WorkItem").ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public WorkItem Find(int id)
        {
            _con.OpenWithRetry();
            try
            {
                var workItem = _con.QueryWithRetry<WorkItem>("SELECT * FROM WorkItem WHERE Id = @id", new { id }).SingleOrDefault();
                var issues = _con.QueryWithRetry<Issue>("SELECT * FROM Issue WHERE WorkItemId = @Id", new { id }).ToList();
                if (workItem != null)
                {
                    workItem.Issues = issues;
                }
                return workItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public WorkItem Add(WorkItem workItem)
        {
            _con.OpenWithRetry();
            var sqlQuery = "INSERT INTO WorkItem (Title, Description, DateCreated, DateDone, Reviewed, PriorityIndex, UserId, TeamId) " +
                       "VALUES(@Title, @Description, @DateCreated, @DateDone, @Reviewed, @PriorityIndex, @UserId, @TeamId)" +
                       "SELECT SCOPE_IDENTITY()";
            try
            {
                var workItemId = _con.QueryWithRetry<int>(sqlQuery, workItem).Single();
                workItem.Id = workItemId;
                return workItem;
            }
            catch (Exception)
            {
                return null;
            }
          
        }

        public bool Update(WorkItem workItem)
        {
            _con.OpenWithRetry();
            var sqlQuery = "UPDATE WorkItem SET Title = @Title, Description = @Description, DateCreated = @DateCreated, DateDone = @DateDone, Reviewed = @Reviewed, UserId = @UserId, TeamId = @TeamId WHERE Id = @id";
            try
            {            
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, workItem);
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove(int id)
        {
            _con.OpenWithRetry();
            var sqlQuery = "DELETE From Issue where WorkItemId = @id DELETE FROM WorkItem WHERE Id = @id";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, new { id });
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
       
        public bool SetStatus(int id, bool status)
        {
            _con.OpenWithRetry();
            var sqlQuery = "";
            var dateDone = DateTime.UtcNow;
            sqlQuery = status ? "UPDATE WorkItem SET DateDone = @DateDone WHERE Id = @Id" : "UPDATE WorkItem SET DateDone = null WHERE Id = @Id";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, new { id, dateDone });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

        public bool SetReviewed(int id)
        {
            _con.OpenWithRetry();
            var sqlQuery = "UPDATE WorkItem SET Reviewed = true WHERE Id = @Id";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, new { id });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }          
        }

        public bool AssignUser(int id, int userId)
        {
            _con.OpenWithRetry();
            var sqlQuery = "UPDATE WorkItem SET UserId = @UserId WHERE Id = @Id";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, new { id, userId });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }          
        }

        public List<WorkItem> GetAllDoneBetweenDates(DateTime startDate, DateTime endDate)
        {
            _con.OpenWithRetry();
            var sqlQuery = "SELECT * FROM WorkItem WHERE DateDone < @endDate AND DateDone > @startDate";
            try
            {
                List<WorkItem> result = _con.QueryWithRetry<WorkItem>(sqlQuery, new { endDate, startDate }).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }           
        }

        public List<WorkItem> Search(string search)
        {
            _con.OpenWithRetry();
            var sqlQuery =
"SELECT * FROM [WorkItem] WHERE Description LIKE @search";
            search = "%" + search + "%";
            try
            {
                var workItems = _con.QueryWithRetry<WorkItem>(sqlQuery, new { search }).ToList();
                return workItems;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
