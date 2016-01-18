using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Win32;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests
{
    [TestFixture]
    public class UserRepoTests
    {

        [OneTimeSetUp]
        public void Seed()
        {
            _dbContext.Add(new User {FirstName = "Åsa", LastName = "Skoog", UserName = "skoog"});
            _dbContext.Add(new User { FirstName = "Jens", LastName = "Backelin", UserName = "backelin" });
            _dbContext.Add(new User { FirstName = "Tobias", LastName = "Balzano", UserName = "balzano" });
        }

        readonly UserRepository _dbContext = new UserRepository();
        [Test]
        public void GetAllUsers()
        {         
            var users = _dbContext.GetAll();
            Assert.That(users != null);
        }

        public void Find()
        {
            
        }

        [Test]
        public void InsertUser()
        {
            var user = new User {FirstName = "Jens", LastName = "Backelin", UserName = "jensbackelin"};
            var addedUser = _dbContext.Add(user);
            Assert.That(addedUser != null);
        }

        [Test]
        public void UpdateUser()
        {
            var user = new User { Id =1, FirstName = "Åsa", LastName = "Skoog", UserName = "skoooooog" };
            var updatedUser =_dbContext.Update(user);
            Assert.That(updatedUser != null);
        }

        //[Test]
        //public void DeleteUser()
        //{

        //    var id = 1;
        //    _dbContext.Remove(id);
        //    Assert.That( != null);
        //}
    }
}
