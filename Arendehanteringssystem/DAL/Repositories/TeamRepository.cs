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
    public class TeamRepository : ITeam
    {
        private IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public Team Add(Team team)
        {
            string query = "INSERT INTO TEAM (Name) VALUES (@Name)";
            team.Id = _con.Query<int>(query, new { team.Name }).Single();
            return team;
        }

        public Team Find(int id)
        {
            string query = "SELECT * FROM Team WHERE Id = @Id";
            return _con.Query<Team>(query, new { id }).Single();
        }

        public List<Team> GetAll()
        {
            return _con.Query<Team>("SELECT * FROM Team").ToList();
        }

        public Team GetTeamWithUser(int id)
        {
            var query = "SELECT * FROM Team WHERE Id = @Id SELECT UserId FROM UserTeam WHERE TeamId = @Id SELECT * FROM User WHERE Id = UserId";
            using(var result = _con.QueryMultiple(query, new { id }))
            {
                Team team = result.Read<Team>().Single();
                team.TeamUsers = result.Read<User>().ToList();
                return team;
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Team Update(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
