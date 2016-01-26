using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.IRepositories;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Repositories
{
    public sealed class TeamRepository : ITeamRepository
    {
        private readonly SqlConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public IEnumerable<Team> GetAll()
        {
            _con.OpenWithRetry();
            try
            {
                return _con.QueryWithRetry<Team>("SELECT * FROM [Team]");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Team Find(int id)
        {
            _con.OpenWithRetry();
            var sqlQueryTeam = "SELECT * FROM [Team] WHERE Id = @id";
            var sqlQueryUser = "SELECT * FROM [User] JOIN [UserTeam] ON TeamId = @id WHERE [User].Id = [UserTeam].UserId";
            try
            {

                var team = _con.QueryWithRetry<Team>(sqlQueryTeam, new { id }).SingleOrDefault();
                var users = _con.QueryWithRetry<User>(sqlQueryUser, new { id }).ToList();

                if (team != null)
                {
                    team.TeamUsers = users;
                }
                return team;
            }            
            catch (Exception)
            {
                return null;
            }
        }

        public Team Add(Team team)
        {
            _con.OpenWithRetry();
            string sqlQuery = "INSERT INTO [Team] (Name) VALUES (@Name) SELECT * FROM [Team] WHERE Id = SCOPE_IDENTITY()";
            try
            {
                var newTeam = _con.QueryWithRetry<Team>(sqlQuery, team).Single();
                team.Id = newTeam.Id;
                return team;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool Update(Team team)
        {
            _con.OpenWithRetry();
            var sqlQuery = "UPDATE [Team] SET Name = @Name WHERE Id = @id";
            try
            {        
                var affectedRows = _con.ExecuteWithRetry(sqlQuery, team);
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
            var sqlQuery = "DELETE FROM [Team] WHERE Id = @id";
            try
            {
                int affectedRows = _con.ExecuteWithRetry(sqlQuery, new { id });
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddTeamMember(int teamId, int userId)
        {
            _con.OpenWithRetry();
            var sqlQuery = "INSERT INTO [UserTeam] (TeamId, UserId) VALUES (@teamId, @userId)";
            try
            {
                int affectedRows = _con.ExecuteWithRetry(sqlQuery, new { teamId, userId });
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveTeamMember(int teamId, int userId)
        {
            _con.OpenWithRetry();
            var sqlQuery = "DELETE FROM [UserTeam] WHERE TeamId = @teamId AND UserId = @userId)";
            try
            {
                int affectedRows = _con.ExecuteWithRetry(sqlQuery, new { teamId, userId });
                return affectedRows != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
