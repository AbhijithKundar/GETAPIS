using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;

namespace AngularAuthYtAPI.Repository.Interface
{
    public interface IMemberRepository
    {
        public List<Member> GetAllMembers();
        public List<MemberModel> GetAllMembers(string? userName);
        public TeamTreeViewModel GetTeamTree(string? userName);
        public TeamTreeViewModel GetAutofillTree(string? userName);
        public decimal GetTotalTeamIncome(string? userName);
        public decimal GetTotalAffiliateIncome(int id);
    }
}
