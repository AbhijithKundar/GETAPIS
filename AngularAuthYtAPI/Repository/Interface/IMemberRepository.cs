using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;

namespace AngularAuthYtAPI.Repository.Interface
{
    public interface IMemberRepository
    {
        public List<Member> GetAllMembers();
        public Member GetMember(string? userName);
        public List<TeamTree> GetAllMembers(string? userName);
        public List<TeamTreeModel> GetTeamTree(int id);
        public TeamTreeViewModel GetAutofillTree(string? userName);
        public decimal GetTotalAffiliateIncome(int id);
    }
}
