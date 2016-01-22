using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAll();
        Team Find(int id);
        Team Add(Team team);
        bool Update(Team team);
        bool Remove(int id);
        bool AddTeamMember(int teamId, int userId);
        bool RemoveTeamMember(int teamId, int userId);
    }
}
