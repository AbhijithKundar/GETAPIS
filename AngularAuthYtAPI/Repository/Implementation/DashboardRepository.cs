using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;
using AngularAuthYtAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AngularAuthYtAPI.Repository.Implementation
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemberRepository _memberRepository;
        public DashboardRepository(AppDbContext context, IMemberRepository memberRepository)
        {
            _context = context;
            _memberRepository = memberRepository;
        }
        public DashboardModel GetDashboard(string userName)
        {
            try
            {
                var member = _context.Members.Where(x => x.Name.Trim().ToLower() == userName.Trim().ToLower()).FirstOrDefault();

                var data = (from m in _context.Members
                            join p in _context.PlanTypes on m.PlanId equals p.Id
                            select new
                            {
                                memberId = m.Id,
                                parentId = m.ParentId,
                                name = m.Name,
                                rate = p.Rate,
                                planId = p.Id
                            }).ToList();

                var dashboardModel = new DashboardModel();
                dashboardModel.PackageValue = data.FirstOrDefault(x => x.memberId == member.Id).rate;

                var memberForUser = data.Where(x => x.parentId == member.Id && x.planId <= member.PlanId).ToList();
                var sumOfDirect = memberForUser.Sum(x => x.rate);
                dashboardModel.DirectIncome = sumOfDirect / 2;
                dashboardModel.AutofillIncome = _memberRepository.GetTotalAffiliateIncome(member.Id);
                dashboardModel.TeamIncome = (decimal)_memberRepository.GetTeamTree(member.Id).Sum(x=>x.TeamIncome);
                dashboardModel.RewardIncome = 0;
                dashboardModel.MagicIncome = 0;

                return dashboardModel;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
