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
        List<Team> GetAll();
        Team Find(int id);
        Team Add(Team team);
        bool Update(Team team);
        bool Remove(int id);
        Team GetTeamWithUser(int id);
        int AddTeamMember(int teamId, int userId);
        int RemoveTeamMember(int teamId, int userId);
    }
}
