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
    public class IssueRepository : IIssueRepository
    {
        private readonly IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public List<Issue> GetAll()
        {
            try
            {
                return _con.Query<Issue>("SELECT * FROM Issue").ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Issue Add(Issue issue)
        {
            var query = "INSERT INTO Issue (Comment, WorkItemId, DateCreated, DateDone, CreatedByUserId)" +
                        "VALUES(@Comment, @WorkItemId, @DateCreated, @DateDone, @CreatedByUserId)" +
                        "SELECT SCOPE_IDENTITY()";
            issue.Id = _con.Query<int>(query, issue).Single();
            return issue;
        }

        public Issue Find(int id)
        {
            string sqlQuery = "SELECT * FROM Issue WHERE Id = @id";

            return _con.Query<Issue>(sqlQuery, new { id }).Single();
        }

        public bool Update(Issue issue)
        {
            try
            {
                var sqlQuery = "UPDATE [Issue] SET Comment = @Comment, DateDone = @DateDone WHERE Id = @id";
                var affectedRows = _con.Execute(sqlQuery, issue);
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
