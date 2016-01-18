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

        public Issue Add(Issue issue)
        {
            var query = "INSERT INTO Issue (Comment, WorkItemId, DateCreated, CreatedByUserId)" +
                        "VALUES(@Comment, @WorkItemId, @DateCreated, @CreatedByUserId)" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
            issue.Id = _con.Query<int>(query, issue).Single();
            return issue;
        }

        public Issue Find(int id)
        {
            return _con.Query("SELECT * FROM User WHERE Id = @id", id).SingleOrDefault();
        }

        public Issue Update(Issue issue)
        {
            var sqlQuery = "UPDATE Issue SET Comment = @Comment, DateDone  = @DateDone, WHERE Id = @Id";
            _con.Execute(sqlQuery, issue);
            return issue; 
        }
    }
}
