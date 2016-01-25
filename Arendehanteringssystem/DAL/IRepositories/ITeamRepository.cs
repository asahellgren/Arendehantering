using System.Collections.Generic;
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
