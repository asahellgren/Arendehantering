using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
