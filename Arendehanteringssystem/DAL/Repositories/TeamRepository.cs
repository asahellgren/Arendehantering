using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IRepositories;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Repositories
{
    public sealed class TeamRepository : ITeam
    {
        private IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public Team Add(Team team)
        {
            string sqlQuery = "INSERT INTO [Team] (Name) VALUES (@Name)";
            team.Id = _con.Execute(sqlQuery, team);
            return team;
        }

        public Team Find(int id)
        {
            string sqlQuery = "SELECT * FROM [Team] WHERE Id = @Id";
            return _con.Query<Team>(sqlQuery, new { id }).Single();
        }

        public List<Team> GetAll()
        {
            return _con.Query<Team>("SELECT * FROM [Team]").ToList();
        }

        public Team GetTeamWithUser(int id)
        {
            var sqlQuery = "SELECT * FROM [Team] WHERE Id = @Id SELECT UserId FROM [UserTeam] WHERE TeamId = @Id SELECT * FROM [User] WHERE Id = UserId";
            using (var result = _con.QueryMultiple(sqlQuery, new { id }))
            {
                Team team = result.Read<Team>().Single();
                team.TeamUsers = result.Read<User>().ToList();
                return team;
            }
        }

        public void Remove(int id)
        {
            var sqlQuery = "DELETE FROM [Team] WHERE Id = @Id";
            _con.Execute(sqlQuery, id);
        }

        public Team Update(Team team)
        {
            var sqlQuery =
             "UPDATE [Team] SET Name = @Name WHERE Id = @Id";
            _con.Execute(sqlQuery, team);
            return team;
        }

        public int AddTeamMember(int teamId, int userId)
        {
            var sqlQuery = "INSERT INTO [UserTeam] (TeamId, UserId) VALUES (@TeamId, @UserId)";
            _con.Execute(sqlQuery, new { teamId, userId });
            return teamId;
        }

        public int RemoveTeamMember(int teamId, int userId)
        {
            var sqlQuery = "DELETE FROM [UserTeam] WHERE TeamId = @TeamId AND UserId = @UserId)";
            _con.Execute(sqlQuery, new { teamId, userId });
            return teamId;
        }


    }
}
