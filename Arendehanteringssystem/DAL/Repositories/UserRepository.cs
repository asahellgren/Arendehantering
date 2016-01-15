﻿using System;
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
            return _con.Query<User>("SELECT * FROM User WHERE Id = @Id").SingleOrDefault();
        }

        public User Add(User user)
        {
            var sqlQuery = "INSERT INTO User (FirstName, LastName, UserName) VALUES(@FirstName, @LastName, @UserName)"  + "SELECT CAST(SCOPE_IDENTITY() as int";
            var Id = _con.Query<int>(sqlQuery, user).Single();
            user.Id = Id;
            return user;
        }

        public User Update(User user)
        {
            var sqlQuery =
             "UPDATE User " +
             "SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName WHERE Id = @Id";
            _con.Execute(sqlQuery, user);
            return user;
        }

        public void Remove(int id)
        {
            var sqlQuery = "DELETE FROM User WHERE Id = @Id";
            _con.Execute(sqlQuery, id);
        }

        public User GetUserWithTeams(int id)
        {
            var sqlQueryUser = "SELECT * FROM User WHERE id = @id");
            var sqlQueryTeam = "SELECT * FROM Team T INNER JOIN UserTeam UT ON T.Id = UT.TeamId WHERE UT.UserId=@Id";
            var user =_con.Query<User>(sqlQueryUser).SingleOrDefault();
            var teams =_con.Query<Team>(sqlQueryTeam).ToList();

            user.Teams = teams;
            return user;




        }
    }
}
