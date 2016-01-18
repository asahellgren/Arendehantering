using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IIssueRepository
    {
        Issue Find(int id);
        Issue Add(Issue issue);
        Issue Update(Issue issue);
    }
}
