using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Win32;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class UserRepoTests
    {
        [Test]
        public void GetAllUsers()
        {
            var dbContext = new UserRepository();
            var users = dbContext.GetAll();
        }
    }
}
