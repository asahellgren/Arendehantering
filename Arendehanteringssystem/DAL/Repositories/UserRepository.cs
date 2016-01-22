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
            try
            {
                return _con.Query<User>("SELECT * FROM [User]").ToList();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public User Find(int id)
        {
            try
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
            catch (Exception)
            {
                return null;
            }
           

        }

        public User Add(User user)
        {
            try
            {
                var sqlQuery = "INSERT INTO [User] (FirstName, LastName, UserName) VALUES(@FirstName, @LastName, @UserName)" + "SELECT SCOPE_IDENTITY()";
                var userId = _con.Query(sqlQuery, user).First();
                user.Id = userId.Id;
                return user;
            }
            catch (Exception)
            {
                return null;
            }          
        }

        public bool Update(User user)
        {
            try
            {
                var sqlQuery = "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName WHERE Id = @Id";
                var affectedRows = _con.Execute(sqlQuery, user);
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Remove(int id)
        {
            try
            {
                var sqlQueryUserTeam = "DELETE FROM [UserTeam] WHERE UserId = @Id";
                var sqlQueryUser = "DELETE FROM [User] WHERE Id = @Id";
                _con.Execute(sqlQueryUserTeam, new { id });
                var affectedRows = _con.Execute(sqlQueryUser, new { id });
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool JoinTeam(int userId, int teamId)
        {
            try
            {
                var sqlQuery = "INSERT INTO [UserTeam] (UserId, TeamId) VALUES(@userId, @teamId)";
                var affectedRows = _con.Execute(sqlQuery, new { userId, teamId });
                return affectedRows == 1;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool LeaveTeam(int userId, int teamId)
        {
            try
            {
                var sqlQuery = "DELETE FROM [UserTeam] WHERE UserId = @userId AND TeamId = @teamId";
                var affectedRows = _con.Execute(sqlQuery, new { userId, teamId });
                return affectedRows == 1;
            }
            catch (Exception)
            {

                return false;
            }

        }
        
    }
}
