using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAll()
        {

        }

        public User Find(int id)
        {
            throw new NotImplementedException();
        }

        public User Add(User user)
        {
            throw new NotImplementedException();
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
