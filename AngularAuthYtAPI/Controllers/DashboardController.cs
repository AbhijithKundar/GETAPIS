using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetDashboard")]
        public async Task<ActionResult> GetDashboard(string? userName)
        {
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == userName.Trim().ToLower());
            if (member is null)
            {
                return BadRequest(new { Message = "Please login again" });
            }

            var data = await (from m in _context.Members
                              join p in _context.PlanTypes on m.PlanId equals p.Id
                              select new
                              {
                                  memberId=m.Id,
                                  parentId = m.ParentId,
                                  name = m.Name,
                                  rate = p.Rate,
                                  planId = p.Id
                              }).ToListAsync();

            if(data is null)
            {
                return BadRequest(new { Message = "No dashboard available" });
            }

            var dashboardModel = new DashboardModel();
           // var packageDetails = _context.PlanTypes.Where(x=>x.Id == member.PlanId).FirstOrDefault();
            dashboardModel.PackageValue = data.FirstOrDefault(x=>x.memberId == member.Id).rate;

             var memberForUser = data.Where(x => x.parentId == member.Id && x.planId <= member.PlanId).ToList();
            var sumOfDirect = memberForUser.Sum(x => x.rate);
            dashboardModel.DirectIncome = sumOfDirect / 2;
            dashboardModel.AutofillIncome = memberForUser.OrderBy(x => x.memberId).Take(6).Sum(x => x.rate) / 10;
            dashboardModel.TeamIncome = 0;
            dashboardModel.RewardIncome = 0;
            dashboardModel.MagicIncome = 0;

            return Ok(dashboardModel);
        }
    }
}
