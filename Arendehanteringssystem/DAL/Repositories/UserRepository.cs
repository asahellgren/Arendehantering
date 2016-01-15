using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IDbConnection _con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arendehantering"].ConnectionString);

        public List<User> GetAll()
        {
            return _con.Query<User>("SELECT * FROM User").ToList();
        }

        public User Find(int id)
        {
            return _con.Query<User>("SELECT * FROM User WHERE")
        }

        public User Add(User user)
        {
            throw new NotImplementedException();
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public List<Team> GetUserWithTeams(int id)
        {
            throw new NotImplementedException();
        }
    }
}
