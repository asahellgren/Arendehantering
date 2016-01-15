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
            throw new NotImplementedException();
        }

        public List<Team> GetAll()
        {
            return _con.Query<Team>("SELECT * FROM User").ToList();
        }

        public List<User> GetTeamWithUser(int id)
        {
            throw new NotImplementedException();
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
