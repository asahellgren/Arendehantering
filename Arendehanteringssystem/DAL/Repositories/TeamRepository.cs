using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IRepositories;
using Dapper;
using System.Configuration;

namespace DAL.Repositories
{
    public class TeamRepository : ITeam
    {
       // string connectionString = ConfigurationManager.Connectionstrings["Arendehantering"].ConnectionString;
        public Team Add(Team team)
        {
            throw new NotImplementedException();
        }

        public Team Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Team> GetAll()
        {
            throw new NotImplementedException();
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
