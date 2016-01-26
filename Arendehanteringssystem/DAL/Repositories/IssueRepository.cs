using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public sealed class IssueRepository : IIssueRepository
    {
        private readonly SqlConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public List<Issue> GetAll()
        {
            _con.OpenWithRetry();
            try
            {
                return _con.QueryWithRetry<Issue>("SELECT * FROM Issue").ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Issue Find(int id)
        {
            _con.OpenWithRetry();
            var sqlQuery = "SELECT * FROM Issue WHERE Id = @id";
            try
            {
                return _con.QueryWithRetry<Issue>(sqlQuery, new { id }).Single();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Issue Add(Issue issue)
        {
            _con.OpenWithRetry();
            var query = "INSERT INTO Issue (Comment, WorkItemId, DateCreated, DateDone, CreatedByUserId)" +
                        "VALUES(@Comment, @WorkItemId, @DateCreated, @DateDone, @CreatedByUserId)" +
                        "SELECT SCOPE_IDENTITY()";
            try
            {
                issue.Id = _con.QueryWithRetry<int>(query, issue).Single();
                return issue;
            }
            catch (Exception)
            {
                return null;
            }         
        }

        public bool Update(Issue issue)
        {
            _con.OpenWithRetry();
            var sqlQuery = "UPDATE [Issue] SET Comment = @Comment, DateDone = @DateDone WHERE Id = @id";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, issue);
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
