using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface ITeam
    {
        List<Team> GetAll();
        Team Find(int id);
        Team Add(Team team);
        Team Update(Team team);
        void Remove(int id);
        Team GetTeamWithUser(int id);
    }
}
