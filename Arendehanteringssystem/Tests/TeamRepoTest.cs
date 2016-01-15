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

        [Test]
        public void GetAllTeams()
        {
            var dbContext = new TeamRepository();
            var result = dbContext.GetAll();
            Assert.NotNull(result);
        }
    }
}
