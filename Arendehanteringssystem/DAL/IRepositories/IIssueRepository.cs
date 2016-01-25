using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IIssueRepository
    {
        Issue Find(int id);
        Issue Add(Issue issue);
        bool Update(Issue issue);
    }
}
