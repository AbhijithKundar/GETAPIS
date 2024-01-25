using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;

namespace AngularAuthYtAPI.Repository.Interface
{
    public interface IMemberRepository
    {
        public List<Member> GetAllMembers();
        public List<MemberModel> GetAllMembers(string? userName);
    }
}
