using System;
using System.CodeDom;
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
        private readonly IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public IEnumerable<Team> GetAll()
        {
            try
            {
                return _con.Query<Team>("SELECT * FROM [Team]");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Team Find(int id)
        {
            try
            {
                var sqlQuery = "SELECT * FROM [Team] WHERE Id = @id SELECT * FROM [User] JOIN [UserTeam] ON TeamId = @id WHERE [User].Id = [UserTeam].UserId";
                using (var result = _con.QueryMultiple(sqlQuery, new { id }))
                {
                    Team team = result.Read<Team>().Single();
                    team.TeamUsers = result.Read<User>().ToList();
                    return team;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Team Add(Team team)
        {
            string sqlQuery = "INSERT INTO [Team] (Name) VALUES (@Name) SELECT SCOPE_IDENTITY()";
            try
            {
                team.Id = _con.Query(sqlQuery, team).Single();
                return team;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool Update(Team team)
        {
            try
            {
                var sqlQuery =
                 "UPDATE [Team] SET Name = @Name WHERE Id = @id";
                int affectedRows = _con.Execute(sqlQuery, team);
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
                var sqlQuery = "DELETE FROM [Team] WHERE Id = @id";
                int affectedRows = _con.Execute(sqlQuery, new { id });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddTeamMember(int teamId, int userId)
        {
            try
            {
                var sqlQuery = "INSERT INTO [UserTeam] (TeamId, UserId) VALUES (@teamId, @userId)";
                int affectedRows = _con.Execute(sqlQuery, new { teamId, userId });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveTeamMember(int teamId, int userId)
        {
            try
            {
                var sqlQuery = "DELETE FROM [UserTeam] WHERE TeamId = @teamId AND UserId = @userId)";
                int affectedRows = _con.Execute(sqlQuery, new { teamId, userId });
                return affectedRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
