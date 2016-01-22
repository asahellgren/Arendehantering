using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public List<User> GetAll()
        {
            return _con.Query<User>("SELECT * FROM [User]").ToList();
        }

        public User Find(int id)
        {
            var sqlQueryUser = "SELECT * FROM [User] WHERE id = @id";
            var sqlQueryTeam = "SELECT * FROM Team T INNER JOIN UserTeam UT ON T.Id = UT.TeamId WHERE UT.UserId=@Id";
            var user = _con.Query<User>(sqlQueryUser, new { id }).SingleOrDefault();
            var teams = _con.Query<Team>(sqlQueryTeam, new { id }).ToList();
            if (user != null)
            {
                user.Teams = teams;
            }
            return user;

        }

        public User Add(User user)
        {
            var sqlQuery = "INSERT INTO [User] (FirstName, LastName, UserName) VALUES(@FirstName, @LastName, @UserName)" + "SELECT Id FROM [User] WHERE Id = SCOPE_IDENTITY()";
            var userId = _con.Query(sqlQuery, user).First();
            user.Id = userId.Id;
            return user;
        }

        public bool Update(User user)
        {
            var sqlQuery =
             "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName WHERE Id = @Id";
            var affectedRows = _con.Execute(sqlQuery, user);
            return affectedRows == 1;
        }

        public bool Remove(int id)
        {
            var sqlQueryUserTeam = "DELETE FROM [UserTeam] WHERE UserId = @Id";
            var sqlQueryUser = "DELETE FROM [User] WHERE Id = @Id";
            _con.Execute(sqlQueryUserTeam, new { id });
            var affectedRows = _con.Execute(sqlQueryUser, new { id });
            return affectedRows != 0;
        }

        public bool JoinTeam(int userId, int teamId)
        {
            var sqlCheckIfRowExsists = "SELECT * FROM [UserTeam] WHERE UserId = @userId AND TeamId = @teamId";
            var result = _con.Query(sqlCheckIfRowExsists, new { userId, teamId });
            if (result.Count() == 0)
            {
                var sqlFindUser = "SELECT Id FROM [User] WHERE Id = @UserId";
                var sqlFindTeam = "SELECT Id FROM [Team] WHERE Id = @TeamId";
                var user =_con.Query(sqlFindUser, new {userId});
                var team = _con.Query(sqlFindUser, new {teamId});

                if (user == null || team == null)
                {
                    return false;
                }
                var sqlQuery = "INSERT INTO [UserTeam] (UserId, TeamId) VALUES(@userId, @teamId)";
                var affectedRows = _con.Execute(sqlQuery, new { userId, teamId });
                return affectedRows == 1;
            }
            else
            {
                return false;
            }
        }

        public bool LeaveTeam(int userId, int teamId)
        {
            var sqlQuery = "DELETE FROM [UserTeam] WHERE UserId = @userId AND TeamId = @teamId";
            var affectedRows = _con.Execute(sqlQuery, new { userId, teamId });
            return affectedRows == 1;
        }




    }
}
