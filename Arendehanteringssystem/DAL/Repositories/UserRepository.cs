using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly SqlConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public List<User> GetAll()
        {
            _con.OpenWithRetry();
            try
            {
                return _con.QueryWithRetry<User>("SELECT * FROM [User]").ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User Find(int id)
        {
            _con.OpenWithRetry();
            var sqlQueryUser = "SELECT * FROM [User] WHERE id = @id";
            var sqlQueryTeam = "SELECT * FROM Team T INNER JOIN UserTeam UT ON T.Id = UT.TeamId WHERE UT.UserId=@Id";

            try
            {
                var user = _con.QueryWithRetry<User>(sqlQueryUser, new { id }).SingleOrDefault();
                var teams = _con.QueryWithRetry<Team>(sqlQueryTeam, new { id }).ToList();

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
            _con.OpenWithRetry();
            var sqlQuery = "INSERT INTO [User] (FirstName, LastName, UserName) VALUES(@FirstName, @LastName, @UserName)" + "SELECT Id from [User] WHERE Id = SCOPE_IDENTITY()";
            try
            {               
                var userId = _con.QueryWithRetry<User>(sqlQuery, user).First();
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
            _con.OpenWithRetry();
            var sqlQuery = "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName WHERE Id = @Id";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, user);
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Remove(int id)
        {
            _con.OpenWithRetry();
            var sqlQuery = "DELETE FROM [User] WHERE Id = @Id";
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

        public bool JoinTeam(int userId, int teamId)
        {
            _con.OpenWithRetry();
            var sqlQuery = "INSERT INTO [UserTeam] (UserId, TeamId) VALUES(@userId, @teamId)";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, new { userId, teamId });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool LeaveTeam(int userId, int teamId)
        {
            _con.OpenWithRetry();
            var sqlQuery = "DELETE FROM [UserTeam] WHERE UserId = @userId AND TeamId = @teamId";
            try
            {
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, new { userId, teamId });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<User> Search(string search)
        {
            _con.OpenWithRetry();
            var sqlQuery =
             "SELECT * FROM [User] WHERE FirstName LIKE @search OR LastName LIKE @search OR UserName LIKE @search";
            search = "%" + search + "%";
            try
            {             
                var users = _con.QueryWithRetry<User>(sqlQuery, new { search }).ToList();
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
