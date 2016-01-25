using System.Collections.Generic;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User Find(int id);
        User Add(User user);
        bool Update(User user);
        bool Remove(int id);
        bool JoinTeam(int userId, int teamId);
        bool LeaveTeam(int userId, int teamId);
        List<User> Search(string search);

    }
}
