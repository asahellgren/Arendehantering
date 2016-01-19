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
    public sealed class TeamRepository: ITeamRepository
    {
        private readonly IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public Team Add(Team team)
        {
            string sqlQuery = "INSERT INTO [Team] (Name) VALUES (@Name) SELECT CAST(SCOPE_IDENTITY() as int)";
            team.Id = _con.Query(sqlQuery, team).Single();
            return team;
        }

        public Team Find(int id)
        {
            string sqlQuery = "SELECT * FROM [Team] WHERE Id = @id";
            return _con.Query<Team>(sqlQuery, new { id }).Single();
        }

        public List<Team> GetAll()
        {
            return _con.Query<Team>("SELECT * FROM [Team]").ToList();
        }

        public Team GetTeamWithUser(int id)
        {
            var sqlQuery = "SELECT * FROM [Team] WHERE Id = @id SELECT * FROM [User] JOIN [UserTeam] ON TeamId = @id WHERE [User].Id = [UserTeam].UserId";
            using (var result = _con.QueryMultiple(sqlQuery, new { id }))
            {
                Team team = result.Read<Team>().Single();
                team.TeamUsers = result.Read<User>().ToList();
                return team;
            }
        }

        public bool Remove(int id)
        {
            var sqlQuery = "DELETE FROM [Team] WHERE Id = @id";
            int affectedRows = _con.Execute(sqlQuery, new { id });
            return affectedRows == 1;
        }

        public bool Update(Team team)
        {
            var sqlQuery =
             "UPDATE [Team] SET Name = @Name WHERE Id = @id";
            int affectedRows = _con.Execute(sqlQuery, team);
            return affectedRows == 1;
        }

        public int AddTeamMember(int teamId, int userId)
        {
            var sqlQuery = "INSERT INTO [UserTeam] (TeamId, UserId) VALUES (@teamId, @userId)";
            _con.Execute(sqlQuery, new { teamId, userId });
            return teamId;
        }

        public int RemoveTeamMember(int teamId, int userId)
        {
            var sqlQuery = "DELETE FROM [UserTeam] WHERE TeamId = @teamId AND UserId = @userId)";
            _con.Execute(sqlQuery, new { teamId, userId });
            return teamId;
        }


    }
}
