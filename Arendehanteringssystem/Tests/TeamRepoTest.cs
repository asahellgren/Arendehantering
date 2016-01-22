using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DAL.Repositories;
using DAL.Entities;

namespace Tests
{
    [TestFixture]
    public class TeamRepoTest
    {
        [OneTimeSetUp]
        public void Seed()
        {
            var dbContext = new TeamRepository();
            dbContext.Add(new Team { Name = "Team1" });
            dbContext.Add(new Team { Name = "Team2" });
            dbContext.Add(new Team { Name = "Team3" });
            dbContext.Add(new Team { Name = "Team4" });
        }

        //[Test]
        //public void GetAllTeamsReturnsAllTeams()
        //{
        //    var dbContext = new TeamRepository();
        //    var result = dbContext.GetAll();
        //    Assert.AreEqual(result.Count, 4);
        //}

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void FindReturnsAUser(int a)
        {
            var dbContext = new TeamRepository();
            var result = dbContext.Find(a);
            Assert.AreEqual(a, result.Id);
        }
    }
}
